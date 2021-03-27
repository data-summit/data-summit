
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
using Microsoft.Extensions.Configuration;

namespace DataSummitHelper.Services
{
    public class DataSummitDocumentsService : IDataSummitDocumentsService
    {
        private readonly IDataSummitDao _dao;
        private readonly IAzureResourcesService _azureResources;

        public DataSummitDocumentsService(IDataSummitDao dao, IAzureResourcesService azureResources)
        {
            _dao = dao;
            _azureResources = azureResources;
        }

        public DataSummitModels.Enums.Document.Type DocumentType(string mimeType)
        {
            var enumType = DataSummitModels.Enums.Document.Type.Unknown;
            switch (mimeType)
            {
                case "DrawingPlanView":
                    enumType = DataSummitModels.Enums.Document.Type.DrawingPlanView;
                    break;
                case "Gantt":
                    enumType = DataSummitModels.Enums.Document.Type.Gantt;
                    break;
                case "Report":
                    enumType = DataSummitModels.Enums.Document.Type.Report;
                    break;
                case "Schematic":
                    enumType = DataSummitModels.Enums.Document.Type.Schematic;
                    break;
            }
            return enumType;
        }

        public DataSummitModels.Enums.Document.Extension DocumentFormat(string mimeFormat)
        {
            var enumFormat = DataSummitModels.Enums.Document.Extension.Unknown;
            switch (mimeFormat)
            {
                case "application/pdf":
                    enumFormat = DataSummitModels.Enums.Document.Extension.PDF;
                    break;
                case "image/jpeg":
                    enumFormat = DataSummitModels.Enums.Document.Extension.JPG;
                    break;
                case "image/x-png":
                    enumFormat = DataSummitModels.Enums.Document.Extension.PNG;
                    break;
                case "image/gif":
                    enumFormat = DataSummitModels.Enums.Document.Extension.GIF;
                    break;
            }
            return enumFormat;
        }

        public DocumentDto GetDocumentsByUrl(string documentUrl)
        {
            var document = _dao.GetDocumentsByUrl(documentUrl);
            var documentDto = new DocumentDto(document);
            return documentDto;
        }

        public async Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId)
        {
            var documents = await _dao.GetAllProjectDocuments(projectId);

            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async Task<List<DocumentDto>> GetProjectDocuments(int projectId)
        {
            var documents = await _dao.GetProjectDocuments(projectId);
            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async System.Threading.Tasks.Task DeleteDocumentProperty(long documentPropertyId)
        {
            await _dao.DeleteTemplateAttribute(documentPropertyId);
        }

    }
}