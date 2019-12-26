using DataSummitHelper;
using DataSummitModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class AddressesController : Controller
    {
        //Connection string determined by Startup.IEnvironment and used privately in dbContext
        //Addresses AddressesService = new Addresses(new DataSummitDbContext(Startup.ConnectionString));
        DataSummitHelper.Addresses AddressesService = new DataSummitHelper.Addresses(new DataSummitDbContext());

        private DataSummitDbContext db = new DataSummitDbContext();
        // GET api/values/5
        [HttpGet("{id}")]
        public string CompanyGet(int id)
        {
            List<DataSummitModels.Addresses> lAddresses = AddressesService.GetAllCompanyAddresses(id, Startup.IsProdEnvironment);
            return JsonConvert.SerializeObject(lAddresses.ToArray());
        }

        [HttpGet("{id}")]
        public string ProjectGet(int id)
        {
            List<DataSummitModels.Addresses> lAddresses = AddressesService.GetAllProjectAddresses(id, Startup.IsProdEnvironment);
            return JsonConvert.SerializeObject(lAddresses.ToArray());
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]DataSummitModels.Addresses address)
        {
            //Create
            return JsonConvert.SerializeObject(AddressesService.CreateAddress(address, Startup.IsProdEnvironment));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DataSummitModels.Addresses address)
        {
            //Update
            AddressesService.UpdateAddress(id, address, Startup.IsProdEnvironment);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            AddressesService.DeleteAddress(id, Startup.IsProdEnvironment);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}
