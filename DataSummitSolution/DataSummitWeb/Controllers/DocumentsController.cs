﻿using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;
using DataSummitService.Interfaces.MachineLearning;
using DataSummitWeb.Params;
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
    [ApiController]
    [Route("api/[controller]")]
    public partial class DocumentsController : Controller
    {
        private readonly IDataSummitDocumentsService _dataSummitDocuments;
        private readonly IAzureResourcesService _azureResourcesService;
        private readonly IClassificationService _classificationService;
        private readonly IObjectDetectionService _objectDetectionService;
        private readonly IDataSummitDocumentsDao _documentsDao;

        public DocumentsController(IDataSummitDocumentsService dataSummitDocuments,
                                   IAzureResourcesService azureResourcesService,
                                   IClassificationService classificationService,
                                   IObjectDetectionService objectDetectionService,
                                   IDataSummitDocumentsDao documentsDao)
        {
            _documentsDao = documentsDao ?? throw new ArgumentNullException(nameof(documentsDao));
            _dataSummitDocuments = dataSummitDocuments ?? throw new ArgumentNullException(nameof(dataSummitDocuments));
            _azureResourcesService = azureResourcesService ?? throw new ArgumentNullException(nameof(azureResourcesService));
            _classificationService = classificationService ?? throw new ArgumentNullException(nameof(classificationService));
            _objectDetectionService = objectDetectionService ?? throw new ArgumentNullException(nameof(objectDetectionService));
        }

        [HttpGet]
        public IActionResult IsRunning()
        {
            return Ok(true);
        }

        [HttpPost("uploadFiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            var uploadedFilesSummary = new List<FileUploadSummaryDto>();
            try
            {
                var tasks = files.Select(file => _azureResourcesService.UploadDataToBlob(file));
                var results = await Task.WhenAll(tasks);

                uploadedFilesSummary = results.ToList();
            }
            catch (Exception ae)
            {
                //TODO log exception
                return Problem(detail: ae.Message, statusCode: 500);
            }
            return Ok(uploadedFilesSummary);
        }

        [HttpPost("determineDocumentType")]
        public async Task<IActionResult> DetermineDocumentType(DetermineDocumentTypeParams determineDocumentTypeParams)
        {
            var blobUrls = determineDocumentTypeParams.BlobUrls;

            try
            {
                // TODO fix this so that it works async without throwing an error
                //var tasks = blobUrls.Select(blobUrl => _classificationService.GetDocumentType(blobUrl));
                //var results = await Task.WhenAll(tasks);

                var documentTypeSummaries = new List<DocumentTypeSummaryDto>();
                foreach (var blobUrl in blobUrls)
                {
                    var documentType = await _classificationService.GetDocumentType(blobUrl);
                    documentTypeSummaries.Add(documentType);
                }

                return Ok(documentTypeSummaries);
            }
            catch (Exception ae)
            {
                return Problem(detail: ae.Message, statusCode: 500);
            }
        }

        [HttpPost("determineDrawingLayout")]
        public async Task<IActionResult> DetermineDrawingLayout(DetermineDrawingLayout determineDrawingLayout)
        {
            try
            {
                var tasks = determineDrawingLayout.BlobUrls.Select(blobUrl => _objectDetectionService.GetDrawingLayout(blobUrl));
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