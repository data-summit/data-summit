using DataSummitService.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Dao.Interfaces
{
    public interface IDataSummitCompaniesDao
    {
        #region Companies
        Task<List<DataSummitModels.DB.Company>> GetAllCompanies();
        Task<DataSummitModels.DB.Company> GetCompanyById(int id);
        Task CreateCompany(DataSummitModels.DB.Company company);
        Task UpdateCompany(DataSummitModels.DB.Company company);
        Task DeleteCompany(int id);
        #endregion
    }
}
