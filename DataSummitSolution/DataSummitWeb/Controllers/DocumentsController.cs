using DataSummitService.Dao.Interfaces;
using DataSummitService.Interfaces;
using DataSummitService.Interfaces.MachineLearning;
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
        private readonly IDataSummitDocumentsService _dataSummitDocuments;
        private readonly IAzureResourcesService _azureResources;
        private readonly IClassificationService _classificationService;
        private readonly IDataSummitDocumentsDao _documentsDao;

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
        public async Task<IActionResult> DetermineDocumentType([FromBody] HashSet<string> blobUrls)
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

                return Ok(documentsAndTypes);
            }
            catch (Exception ae)
            {
                return Problem(detail: ae.Message, statusCode: 500);
            }
        }
    }
}