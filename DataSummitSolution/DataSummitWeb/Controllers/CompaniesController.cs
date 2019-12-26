using DataSummitHelper;
using DataSummitModels;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        DataSummitHelper.Companies companiesService = new DataSummitHelper.Companies(new DataSummitDbContext());

        // GET api/companies
        [HttpGet]
        public string Get()
        {
            List<DataSummitModels.Companies> companies = companiesService.GetAllCompanies();
            return JsonConvert.SerializeObject(companies.ToArray());
        }

        // GET api/companies/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            DataSummitModels.Companies company = companiesService.GetCompanyById(id);
            return JsonConvert.SerializeObject(company);
        }

        // POST api/Company
        [HttpPost]
        public string Post([FromBody]DataSummitModels.Companies company)
        {
            return JsonConvert.SerializeObject(companiesService.CreateCompany(company));
        }

        // PUT api/Company/5
        [HttpPut("{id}")]
        public void Put([FromBody]DataSummitModels.Companies company)
        {
            companiesService.UpdateCompany(company);
        }

        // DELETE api/Company/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            companiesService.DeleteCompany(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}
