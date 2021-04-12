using DataSummitService.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Dao.Interfaces
{
    public interface IDataSummitPropertiesDao
    {
        #region Properties
        Task<DataSummitModels.DB.Property> GetPropertyById(int id);
        Task<List<DocumentPropertyDto>> GetDocumentPropertiesByDocumentId(int documentId);
        Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue);
        Task<bool> DeleteProperty(long propertyId);
        #endregion
    }
}
