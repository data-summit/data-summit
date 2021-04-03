using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitModels.DB;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public partial class DocumentsController : Controller
    {
        private readonly IDataSummitDocumentsDao _documentsDao;
        private readonly IDataSummitDocumentsService _dataSummitDocuments;
        private readonly IAzureResourcesService _azureResources;
        private readonly IClassificationService _classificationService;

        public DocumentsController(IDataSummitDocumentsService dataSummitProjects, 
                                   IAzureResourcesService azureResources,
                                   IClassificationService classificationService, 
                                   IDataSummitDocumentsDao documentsDao)
        {
            _documentsDao = documentsDao ?? throw new ArgumentNullException(nameof(documentsDao));
            _dataSummitDocuments = dataSummitProjects ?? throw new ArgumentNullException(nameof(dataSummitProjects));
            _azureResources = azureResources ?? throw new ArgumentNullException(nameof(azureResources));
            _classificationService = classificationService ?? throw new ArgumentNullException(nameof(classificationService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var documents = await _dataSummitDocuments.GetProjectDocuments(id);
            return Ok(documents);
        }


        [HttpPost("uploadFiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            var uploadedFileURLs = new HashSet<string>();
            try
            {
                files.ForEach(async file =>
                {
                    if (file != null)
                    {
                        var uploadedFileUrl = await _azureResources.UploadDataToBlob(file);
                        uploadedFileURLs.Add(uploadedFileUrl);
                    }
                });
            }
            catch (Exception ae)
            {
                //TODO log exception
                return Problem(detail: ae.Message, statusCode: 500);
            }
            return Ok(uploadedFileURLs);
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
                    List<KeyValuePair<string, string>> additionalMetaData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("DocumentType", documentTypeEnum.ToString()),
                        new KeyValuePair<string, string>("DocumentTypeConfidence", typeConfidence.ToString())
                    };

                    await _azureResources.AddMetadataToBlob(blobUrl, additionalMetaData);

                    //Persist in database
                    var doc = _documentsDao.GetDocumentsByUrl(blobUrl);
                    doc.DocumentType = new DocumentType()
                    {
                        Name = documentTypeEnum.ToString(),
                        DocumentTypeId = (byte)documentTypeEnum
                    };

                    doc.AzureConfidence = (decimal)typeConfidence;
                    _documentsDao.UpdateDocument(doc);

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
        public async Task DetermineDrawingComponents([FromBody] HashSet<string> blobUrls)
        {
            try
            {
                foreach (var blobUrl in blobUrls)
                {
                    await _dataSummitDocuments.UpdateDocumentFeature(blobUrl);
                }
            }
            catch (Exception ae)
            {
                //TODO log exception
                return;
            }
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            _documentsDao.DeleteDocument(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}