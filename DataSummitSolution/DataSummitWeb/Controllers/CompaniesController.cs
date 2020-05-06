using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using DataSummitWeb.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;

        public CompaniesController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        // GET api/companies
        [HttpGet]
        public async Task<string> GetAllCompanies()
        {
            var companyDtos = await _dataSummitHelper.GetAllCompanies();
            var companies = companyDtos.Select(c => Company.FromDto(c))
                .ToList();
            return JsonConvert.SerializeObject(companies.ToArray());
        }

        // GET api/companies/{id}
        [HttpGet("{id}")]
        public string GetCompanyById(int id)
        {
            var company = _dataSummitHelper.GetCompanyById(id);
            return JsonConvert.SerializeObject(company);
        }

        // POST api/Company/create
        [HttpPost("create")]
        public async Task CreateNewCompany([FromBody]Company company)
        {
            await _dataSummitHelper.CreateCompany(company.ToDto());
        }

        // PUT api/Company/update
        [HttpPut("update")]
        public async Task UpdateCompany([FromBody]CompanyDto company)
        {
            await _dataSummitHelper.UpdateCompany(company);
        }

        // DELETE api/Company/delete
        [HttpDelete("delete")]
        public async Task DeleteCompany([FromBody]int companyId)
        {
            await _dataSummitHelper.DeleteCompany(companyId);
        }
    }
}
