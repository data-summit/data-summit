using DataSummitHelper.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitCompaniesService
    {
        #region Companies
        Task<List<CompanyDto>> GetAllCompanies();
        Task<CompanyDto> GetCompanyById(int id);
        Task CreateCompany(CompanyDto companyDto);
        Task UpdateCompany(CompanyDto companyDto);
        Task DeleteCompany(int id);
        #endregion
    }
}