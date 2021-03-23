
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataSummitHelper.Services
{
    public class DataSummitCompaniesService : IDataSummitCompaniesService
    {
        private readonly IDataSummitDao _dao;
        private readonly IAzureResourcesService _azureResources;

        public DataSummitCompaniesService(IDataSummitDao dao, IAzureResourcesService azureResources)
        {
            _dao = dao;
            _azureResources = azureResources;
        }

        #region Companies
        public async Task<List<CompanyDto>> GetAllCompanies()
        {
            var companies = await _dao.GetAllCompanies();
            var companyDtos = companies.Select(c => new CompanyDto(c))
                .ToList();

            return companyDtos;
        }

        public async Task<CompanyDto> GetCompanyById(int id)
        {
            var  company = await _dao.GetCompanyById(id);
            return new CompanyDto(company);
        }

        public async Task CreateCompany(CompanyDto companyDto)
        {
            var company = companyDto.ToCompany();
            await _dao.CreateCompany(company);
            company.ResourceGroup = await _azureResources.CreateResourceGroup(company.Name);
        }

        public async Task UpdateCompany(CompanyDto company)
        {
            var companyDto = company.ToCompany();
            await _dao.UpdateCompany(companyDto);
            companyDto.ResourceGroup = await _azureResources.UpdateResourceGroup(companyDto.ResourceGroup, companyDto.Name);
        }

        public async Task DeleteCompany(int id)
        {
            var company = await _dao.GetCompanyById(id);
            await _dao.DeleteCompany(id);
            await _azureResources.DeleteResourceGroup(company.ResourceGroup);
        }
        #endregion
    }
}