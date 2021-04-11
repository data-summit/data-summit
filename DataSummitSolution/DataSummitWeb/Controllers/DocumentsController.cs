using DataSummitService.Interfaces;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAzureResourcesService _azureService;

        public DocumentsController(IDataSummitDocumentsService dataSummitDocuments, 
                                   IAzureResourcesService azureResources,
                                   IClassificationService classificationService, 
                                   IDataSummitDocumentsDao documentsDao)
        {
            _documentsDao = documentsDao ?? throw new ArgumentNullException(nameof(documentsDao));
            _dataSummitDocuments = dataSummitDocuments ?? throw new ArgumentNullException(nameof(dataSummitDocuments));
            _azureResources = azureResources ?? throw new ArgumentNullException(nameof(azureResources));
            _classificationService = classificationService ?? throw new ArgumentNullException(nameof(classificationService));
        }

        [HttpGet]
        public IActionResult IsRunning()
        {
            return Ok(true);
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
        public async Task<IDictionary<string, string>> DetermineDocumentType([FromBody] HashSet<string> blobUrls)
        {
            IDictionary<string, string> documentsAndTypes = new Dictionary<string, string>();
            try
            {
                List<Task> tasks = new List<Task>();
                
                foreach (var url in blobUrls)
                {
                    var kv = await _classificationService.GetDocumentType(url);
                    documentsAndTypes.Add(kv);
                }

                //blobUrls.ToList().ForEach(url =>
                //    tasks.Add(Task.Run(async () =>
                //    {
                //        documentsAndTypes.Add(await _classificationService.GetDocumentType(url));
                //    }))
                //);
                //await Task.WhenAll(tasks.ToArray());

                //foreach (var blobUrl in blobUrls)
                //{
                //    var documentTypeClassification = await _classificationService.GetPrediction(blobUrl, "DocumentType", "Classification");
                //    var documentTypeEnum = _dataSummitDocuments.DocumentType(documentTypeClassification.TagName);
                //    var typeConfidence = Math.Round(documentTypeClassification.Probability, 3);

                //    // Update blob metadata
                //    List<KeyValuePair<string, string>> additionalMetaData = new List<KeyValuePair<string, string>>
                //    {
                //        new KeyValuePair<string, string>("DocumentType", documentTypeEnum.ToString()),
                //        new KeyValuePair<string, string>("DocumentTypeConfidence", typeConfidence.ToString())
                //    };

                //    await _azureResources.AddMetadataToBlob(blobUrl, additionalMetaData);

                //    //Persist in database
                //    var doc = _documentsDao.GetDocumentsByUrl(blobUrl);
                //    doc.DocumentType = new DocumentType()
                //    {
                //        Name = documentTypeEnum.ToString(),
                //        DocumentTypeId = (byte)documentTypeEnum
                //    };

                //    doc.AzureConfidence = (decimal)typeConfidence;
                //    _documentsDao.UpdateDocument(doc);

                //    documentsAndTypes.Add(blobUrl, documentTypeEnum.ToString());
                //}
            }
            catch (Exception ae)
            {
                //TODO log exception
            }
            return documentsAndTypes;
        }

                var uploadedFileURLs = new HashSet<string>(results);
                return Ok(uploadedFileURLs);
            }
            catch (Exception ae)
            {
                return Problem(detail: ae.Message, statusCode: 500);
            }
        }
    }
}