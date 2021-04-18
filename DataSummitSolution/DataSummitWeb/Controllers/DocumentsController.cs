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
        private readonly IAzureResourcesService _azureResourcesService;
        private readonly IClassificationService _classificationService;
        private readonly IDataSummitDocumentsDao _documentsDao;

        public DocumentsController(IDataSummitDocumentsService dataSummitDocuments,
                                   IAzureResourcesService azureResourcesService,
                                   IClassificationService classificationService,
                                   IDataSummitDocumentsDao documentsDao)
        {
            _documentsDao = documentsDao ?? throw new ArgumentNullException(nameof(documentsDao));
            _dataSummitDocuments = dataSummitDocuments ?? throw new ArgumentNullException(nameof(dataSummitDocuments));
            _azureResourcesService = azureResourcesService ?? throw new ArgumentNullException(nameof(azureResourcesService));
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
                var tasks = files.Select(file => _azureResourcesService.UploadDataToBlob(file));
                var results = await Task.WhenAll(tasks);
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
            try
            {
                var tasks = blobUrls.Select(blobUrl => _classificationService.GetDocumentType(blobUrl));
                var results = await Task.WhenAll(tasks);

                return Ok(results);
            }
            catch (Exception ae)
            {
                return Problem(detail: ae.Message, statusCode: 500);
            }
        }
    }
}