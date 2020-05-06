using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProfileAttributesController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ProfileAttributesController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        private DataSummitDbContext db = new DataSummitDbContext();
        
        // GET api/profileAttributes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var profileAttributes = await _dataSummitHelper.GetTemplateAttribtes(id);
            return Ok(profileAttributes);
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]ProfileAttributes profileAttribute)
        {
            //Create
            //return JsonConvert.SerializeObject(
            //    profileAttributeService.CreateProfileAttribute(profileAttribute));
            return null;
        }

        // PUT api/profileAttributes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]ProfileAttributes profileAttribute)
        {
            //Update
            //profileAttributeService.UpdateProfileAttribute(id, profileAttribute);
            return;
        }

        // DELETE api/profileAttributes/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //profileAttributeService.DeleteProfileAttribute(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}