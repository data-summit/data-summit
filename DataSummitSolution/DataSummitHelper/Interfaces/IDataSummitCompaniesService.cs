using DataSummitService.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IDataSummitCompaniesService
    {
        Task<List<CompanyDto>> GetAllCompanies();
        Task<CompanyDto> GetCompanyById(int id);
        Task CreateCompany(CompanyDto companyDto);
        Task UpdateCompany(CompanyDto companyDto);
        Task DeleteCompany(int id);
    }
}