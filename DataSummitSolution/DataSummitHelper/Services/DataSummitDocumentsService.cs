
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
        private readonly IDataSummitAzureUrlsDao _azureDao;

        public DataSummitDocumentsService(IDataSummitDocumentsDao documentsDao,
                                          IDataSummitTemplateAttributesDao templateAttributesDao,
                                          IDataSummitAzureUrlsDao azureDao,
                                          IAzureResourcesService azureResources)
        {
            _documentsDao = documentsDao;
            _templateAttributesDao = templateAttributesDao;
            _azureResources = azureResources;
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

        /// <summary>
        ///  This is tech-debt and in a future ticket/iteration we should put the object(string) in a class and have 
        ///  a ToEnum() method in that class.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>DrawingLayout enum</returns>
        public DrawingLayout GetDrawingLayoutEnum(string itemName)
        {
            var enumComponent = DrawingLayout.Unknown;
            switch (itemName)
            {
                case "DrawingContent":
                    enumComponent = DrawingLayout.DrawingContent;
                    break;
                case "Notes":
                    enumComponent = DrawingLayout.Notes;
                    break;
                case "TitleBox":
                    enumComponent = DrawingLayout.TitleBox;
                    break;
            }
            return enumComponent;
        }

        public async Task<List<DocumentFeature>> GetDocumentText(string url)
        {
            var features = new List<DocumentFeature>();
            var azureFunction = await _azureDao.GetAzureFunctionUrlByName("RecogniseTextAzure");
            


            return features;
        }

        public DocumentDto GetDocumentDtoByUrl(string documentUrl)
        {
            var document = _documentsDao.GetDocumentByUrl(documentUrl);
            var documentDto = new DocumentDto(document);
            return documentDto;
        }

        public Document GetDocumentByUrl(string documentUrl) => _documentsDao.GetDocumentByUrl(documentUrl);

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