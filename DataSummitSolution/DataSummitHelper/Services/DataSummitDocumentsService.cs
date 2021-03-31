
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using DataSummitModels.Enums;
using Microsoft.Extensions.Configuration;

namespace DataSummitHelper.Services
{
    public class DataSummitDocumentsService : IDataSummitDocumentsService
    {
        private readonly IDataSummitDocumentsDao _documentsDao;
        private readonly IDataSummitTemplateAttributesDao _templateAttributesDao;
        private readonly IAzureResourcesService _azureResources;

        public DataSummitDocumentsService(IDataSummitDocumentsDao documentsDao,
                                          IDataSummitTemplateAttributesDao templateAttributesDao,
                                          IAzureResourcesService azureResources)
        {
            _documentsDao = documentsDao;
            _templateAttributesDao = templateAttributesDao;
            _azureResources = azureResources;
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

        public Document GetDocumentByUrl(string documentUrl)
        {
            var document = _documentsDao.GetDocumentsByUrl(documentUrl);
            return document;
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