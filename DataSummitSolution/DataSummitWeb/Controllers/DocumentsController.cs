using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitModels.DB;
using DataSummitModels.DTO;
using DataSummitModels.Enums;
using DataSummitModels.Models;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public partial class DocumentsController : Controller
    {
        private readonly IDataSummitDao _dao;
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly IDataSummitDocumentsService _dataSummitDocuments;
        private readonly IAzureResourcesService _azureResources;
        private readonly IClassificationService _classificationService;
        private readonly IObjectDetectionService _objectDetectionService;

        public DocumentsController(IDataSummitDocumentsService dataSummitProjects, IAzureResourcesService azureResources,
            IClassificationService classificationService, IObjectDetectionService objectDetectionService, 
            IDataSummitHelperService dataSummitHelper, IDataSummitDao dao)
        {
            _dao = dao ?? throw new ArgumentNullException(nameof(dao));
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _dataSummitDocuments = dataSummitProjects ?? throw new ArgumentNullException(nameof(dataSummitProjects));
            _azureResources = azureResources ?? throw new ArgumentNullException(nameof(azureResources));
            _classificationService = classificationService ?? throw new ArgumentNullException(nameof(classificationService));
            _objectDetectionService = objectDetectionService ?? throw new ArgumentNullException(nameof(objectDetectionService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var documents = await _dataSummitDocuments.GetProjectDocuments(id);
            return Ok(documents);
        }


        [HttpPost("uploadFiles")]
        public async Task<HashSet<string>> UploadFiles(ICollection<IFormFile> files)
        {
            var uploadedFileURLs = new HashSet<string>();
            try
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var uploadedFileUrl = await _azureResources.UploadDataToBlob(file);
                        uploadedFileURLs.Add(uploadedFileUrl);
                    }
                }
            }
            catch (Exception ae)
            {
                //TODO log exception
                return null;
            }
            return uploadedFileURLs;
        }

        [HttpPost("determineDocumentType")]
        public async Task<Dictionary<string, string>> DetermineDocumentType([FromBody] HashSet<string> blobUrls)
        {
            var documentsAndTypes = new Dictionary<string, string>();
            try
            {
                foreach (var blobUrl in blobUrls)
                {
                    var documentTypeClassification = await _classificationService.GetPrediction(blobUrl, "DocumentType", "Classification");
                    var documentTypeEnum = _dataSummitDocuments.DocumentType(documentTypeClassification.TagName);
                    var typeConfidence = Math.Round(documentTypeClassification.Probability, 3);

                    // Update blob metadata
                    List<KeyValuePair<string, string>> additionalMetaData = new List<KeyValuePair<string, string>>();
                    additionalMetaData.Add(new KeyValuePair<string, string>("DocumentType", documentTypeEnum.ToString()));
                    additionalMetaData.Add(new KeyValuePair<string, string>("DocumentTypeConfidence", typeConfidence.ToString()));

                    await _azureResources.AddMetadataToBlob(blobUrl, additionalMetaData);

                    //Persist in database
                    var doc = _dao.GetDocumentsByUrl(blobUrl);
                    doc.DocumentType = new DocumentType()
                    {
                        Name = documentTypeEnum.ToString(),
                        DocumentTypeId = (byte)documentTypeEnum
                    };
                    //doc.DocumentType.DocumentTypeId = (byte)documentTypeEnum;
                    doc.AzureConfidence = (decimal)typeConfidence;
                    _dao.UpdateDocument(doc);

                    documentsAndTypes.Add(blobUrl, documentTypeEnum.ToString());
                }
            }
            catch (Exception ae)
            {
                //TODO log exception
            }
            return documentsAndTypes;
        }

        [HttpPost("determineDrawingComponents")]
        public async void DetermineDrawingComponents([FromBody] HashSet<string> blobUrls)
        {
            try
            {
                foreach (var blobUrl in blobUrls)
                {
                    var doc = _dataSummitDocuments.GetDocumentByUrl(blobUrl);      // If document is run later in the method a _dao disposed error occurs
                    var features = new List<DocumentFeature>();
                    features.AddRange(doc.DocumentFeatures.ToList());

                    var documentFeatures = await _objectDetectionService.GetPrediction(blobUrl, "DrawingLayout", "ObjectDetection", 0.05);
                    if (documentFeatures != null && documentFeatures.Count > 0)
                    { 
                        foreach (var feature in documentFeatures)
                        {
                            var docFeature = new DocumentFeature()
                            {
                                Value = feature.TagName,
                                Confidence = Math.Round(feature.Probability, 5),
                                Vendor = "Microsoft Custom Vision",
                                Left = (long)Math.Round(feature.BoundingBox.Min.X, 0),
                                Top = (long)Math.Round(feature.BoundingBox.Max.Y, 0),
                                Width = (long)Math.Round(feature.BoundingBox.Max.X - feature.BoundingBox.Min.X, 0),
                                Height = (long)Math.Round(feature.BoundingBox.Max.Y - feature.BoundingBox.Min.Y, 0)
                            };
                            features.Add(docFeature);
                        }
                    }

                    //Persists in database
                    doc.DocumentFeatures  = features;
                    _dataSummitDocuments.UpdateDocument(doc);
                }
            }
            catch (Exception ae)
            {
                //TODO log exception
                return;
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] DataSummitModels.DB.Document project)
        {
            //Update
            //_dataSummitProject.UpdateDocument(id, project);
            return;
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            _dao.DeleteDocument(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}