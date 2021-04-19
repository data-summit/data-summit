
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;
using DataSummitService.Interfaces.MachineLearning;
using DataSummitModels.DB;
using DataSummitModels.Enums;
using Microsoft.Extensions.Configuration;

namespace DataSummitService.Services
{
    public class DataSummitDocumentsService : IDataSummitDocumentsService
    {
        private readonly IDataSummitDocumentsDao _documentsDao;
        private readonly IDataSummitTemplateAttributesDao _templateAttributesDao;
        private readonly IAzureResourcesService _azureResources;
        private readonly IObjectDetectionService _objectDetectionService;
        private readonly IDataSummitAzureUrlsDao _azureDao;

        public DataSummitDocumentsService(IDataSummitDocumentsDao documentsDao,
                                          IDataSummitTemplateAttributesDao templateAttributesDao,
                                          IObjectDetectionService objectDetectionService,
                                          IDataSummitAzureUrlsDao azureDao,
                                          IAzureResourcesService azureResources)
        {
            _documentsDao = documentsDao;
            _templateAttributesDao = templateAttributesDao;
            _azureResources = azureResources;
            _objectDetectionService = objectDetectionService ?? throw new ArgumentNullException(nameof(objectDetectionService));
            _azureDao = azureDao ?? throw new ArgumentNullException(nameof(azureDao));

        }

        public DocumentContentType DocumentType(string mimeType)
        {
            var enumType = DocumentContentType.Unknown;
            switch (mimeType)
            {
                case "DrawingPlanView":
                    enumType = DocumentContentType.DrawingPlanView;
                    break;
                case "Gantt":
                    enumType = DocumentContentType.Gantt;
                    break;
                case "Report":
                    enumType = DocumentContentType.Report;
                    break;
                case "Schematic":
                    enumType = DocumentContentType.Schematic;
                    break;
            }
            return enumType;
        }

        public DocumentExtension DocumentFormat(string mimeFormat)
        {
            var enumFormat = DocumentExtension.Unknown;
            switch (mimeFormat)
            {
                case "application/pdf":
                    enumFormat = DocumentExtension.PDF;
                    break;
                case "image/jpeg":
                    enumFormat = DocumentExtension.JPG;
                    break;
                case "image/x-png":
                    enumFormat = DocumentExtension.PNG;
                    break;
                case "image/gif":
                    enumFormat = DocumentExtension.GIF;
                    break;
            }
            return enumFormat;
        }

        public DocumentDto GetDocumentDtoByUrl(string documentUrl)
        {
            var document = _documentsDao.GetDocumentsByUrl(documentUrl);
            var documentDto = new DocumentDto(document);
            return documentDto;
        }

        public Document GetDocumentByUrl(string documentUrl) => _documentsDao.GetDocumentsByUrl(documentUrl);

        public async Task UpdateDocumentFeature(string documentUrl)
        {

            var azureFunction = await _azureDao.GetAzureFunctionUrlByName("ObjectDetection");
            var azureAI = await _azureDao.GetMLUrlByNameAsync("DrawingLayout");
            var documentPredictions = await _objectDetectionService.GetPrediction(documentUrl, azureFunction, azureAI, 0.05);
            if (documentPredictions?.Any() ?? false)
            {
                documentPredictions.ForEach(async docPred =>
                {
                    var documentFeature = new DocumentFeature
                    {
                        Value = docPred.TagName,
                        Confidence = (decimal)Math.Round(docPred.Probability, 5),
                        Vendor = "Microsoft Custom Vision",
                        Left = (long)Math.Round(docPred.BoundingBox.Min.X, 0),
                        Top = (long)Math.Round(docPred.BoundingBox.Max.Y, 0),
                        Width = (long)Math.Round(docPred.BoundingBox.Max.X - docPred.BoundingBox.Min.X, 0),
                        Height = (long)Math.Round(docPred.BoundingBox.Max.Y - docPred.BoundingBox.Min.Y, 0)
                    };

                    //Persist in database
                    await _documentsDao.UpdateDocumentFeature(documentUrl, documentFeature);
                });
            }
        }

        public async Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId)
        {
            var documents = await _documentsDao.GetAllProjectDocuments(projectId);

            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async Task<List<DocumentDto>> GetProjectDocuments(int projectId)
        {
            var documents = await _documentsDao.GetProjectDocuments(projectId);
            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async Task DeleteDocumentProperty(long documentPropertyId)
        {
            await _templateAttributesDao.DeleteTemplateAttribute(documentPropertyId);
        }

        public void UpdateDocument(Document document)
        {
            _documentsDao.UpdateDocument(document);
        }
    }
}