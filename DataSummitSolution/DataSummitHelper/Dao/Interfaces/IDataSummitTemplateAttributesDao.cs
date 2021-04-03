using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitTemplateAttributesDao
    {
        #region Template Attributes
        Task<List<DataSummitModels.DB.TemplateAttribute>> GetAttribtesForTemplateId(int templateId);
        Task DeleteTemplateAttribute(long templateAttributeId);
        Task<List<DataSummitModels.DB.TemplateAttribute>> GetAttributesForDocumentId(int documentId);
        #endregion
    }
}
