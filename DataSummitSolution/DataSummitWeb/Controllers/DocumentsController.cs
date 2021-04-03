using DataSummitService.Interfaces;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public partial class DocumentsController : Controller
    {
        private readonly IAzureResourcesService _azureService;

        public DocumentsController(IAzureResourcesService azureResources)
        {
            _azureService = azureResources ?? throw new ArgumentNullException(nameof(azureResources));
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
                        var uploadedFileUrl = await _azureService.UploadDataToBlob(file);
                        uploadedFileURLs.Add(uploadedFileUrl);
                    }
                });
            }
            catch (Exception ae)
            {
                return Problem(detail: ae.Message, statusCode: 500);
            }
            return Ok(uploadedFileURLs);
        }
    }
}