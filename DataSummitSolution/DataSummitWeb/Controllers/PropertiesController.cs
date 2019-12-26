using DataSummitHelper;
using DataSummitModels;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class PropertiesController : Controller
    {
        //Connection string determined by Startup.IEnvironment and used privately in dbContext
        //DataSummitHelper.Properties propertiesService = new DataSummitHelper.Properties(new DataSummitDbContext(localDbConnectionString));
        DataSummitHelper.Properties propertiesService = new DataSummitHelper.Properties(new DataSummitDbContext());

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "";
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]DataSummitModels.Properties properties)
        {
            //Create
            return JsonConvert.SerializeObject(propertiesService.CreateProperty(properties, Startup.IsProdEnvironment));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DataSummitModels.Properties properties)
        {
            //Update
            propertiesService.UpdateProperty(id, properties, Startup.IsProdEnvironment);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public string Delete(long id)
        {
            propertiesService.DeleteProperty(id, Startup.IsProdEnvironment);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}