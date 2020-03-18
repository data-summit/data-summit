using DataSummitModels.DB;
using DataSummitModels.DTO;
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
            DrawingData dd = propertiesService.GetAllDrawingProperties(id);
            return JsonConvert.SerializeObject(dd);
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]DataSummitModels.DB.Properties properties)
        {
            //Create
            return JsonConvert.SerializeObject(propertiesService.CreateProperty(properties));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DataSummitModels.DB.Properties properties)
        {
            //Update
            propertiesService.UpdateProperty(id, properties);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public string Delete(long id)
        {
            propertiesService.DeleteProperty(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}