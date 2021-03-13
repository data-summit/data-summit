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
    public class TemplateAttributesController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;

        public TemplateAttributesController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        private DataSummitDbContext db = new DataSummitDbContext();
        
        // GET api/templateAttributes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var templateAttributes = await _dataSummitHelper.GetTemplateAttribtes(id);
            return Ok(templateAttributes);
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]TemplateAttributes templateAttribute)
        {
            //Create
            //return JsonConvert.SerializeObject(
            //    templateAttributeService.CreateTemplateAttribute(templateAttribute));
            return null;
        }

        // PUT api/templateAttributes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]TemplateAttributes templateAttribute)
        {
            //Update
            //templateAttributeService.UpdateTemplateAttribute(id, templateAttribute);
            return;
        }

        // DELETE api/templateAttributes/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //templateAttributeService.DeleteTemplateAttribute(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}