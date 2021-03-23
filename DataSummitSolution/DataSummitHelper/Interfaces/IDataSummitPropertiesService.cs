using DataSummitHelper.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitPropertiesService
    {
        Task UpdateDocumentPropertyValue(DocumentPropertyDto documentProperty);
        Task<List<DocumentPropertyDto>> GetDocumentProperties(int documentId);
        Task DeleteProperty(long id);
    }
}