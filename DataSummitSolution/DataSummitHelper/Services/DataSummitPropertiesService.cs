using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;

namespace DataSummitHelper.Services
{
    public class DataSummitPropertiesService : IDataSummitPropertiesService
    {
        private readonly IDataSummitPropertiesDao _dao;

        public DataSummitPropertiesService(IDataSummitPropertiesDao dao)
        {
            _dao = dao;
        }

        public async Task UpdateDocumentPropertyValue(DocumentPropertyDto documentProperty)
        {
            await _dao.UpdateDocumentPropertyValue(documentProperty.SentenceId, documentProperty.WordValue);
        }

        public async Task<List<DocumentPropertyDto>> GetDocumentProperties(int documentId)
        {
            var documentProperties = await _dao.GetDocumentPropertiesByDocumentId(documentId);
            var documentPropertyDtos = documentProperties.Select(d => new DocumentPropertyDto(d))
                .ToList();

            return documentPropertyDtos;
        }

        public async Task DeleteProperty(long id)
        {
            await _dao.DeleteProperty(id);
        }
    }
}