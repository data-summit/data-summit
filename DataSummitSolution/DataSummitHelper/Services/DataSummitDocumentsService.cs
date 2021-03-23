
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
        private readonly IConfiguration _configuration;

        public DataSummitDocumentsService(IDataSummitDao dao, IConfiguration configuration)
        {
            _dao = dao;
            _configuration = configuration;
        }

        public async Task<bool> IsDocument(int documentId)
        {
            var documentClassification = _configuration["DocumentClassification"];

            //TODO make http call to //"https://documentlayout.cognitiveservices.azure.com/" and pass data to ML project

            return false;
        }

        public async System.Threading.Tasks.Task DeleteDocumentProperty(long documentPropertyId)
        {
            await _dao.DeleteTemplateAttribute(documentPropertyId);
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
    }
}