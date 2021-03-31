using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitProjectsDao
    {
        #region Projects
        Task<List<DataSummitModels.DB.Project>> GetAllCompanyProjects(int companyId);
        Task<DataSummitModels.DB.Project> GetProjectById(int id);
        Task CreateProject(DataSummitModels.DB.Project company);
        Task UpdateProjectName(DataSummitModels.DB.Project company);
        Task DeleteProject(int id);
        #endregion
    }
}
