using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitTemplatesDao
    {
        #region Templates
        Task<List<DataSummitModels.DB.TemplateVersion>> GetCompanyTemplateVersions(int companyId);
        Task<List<DataSummitModels.DB.TemplateVersion>> GetProjectTemplateVersions(int projectId);
        #endregion
    }
}
