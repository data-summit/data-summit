using DataSummitModels.DB;
using DataSummitModels.DTO;
using DataSummitWeb.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataSummitHelper;
using System.Linq;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Services;
using DataSummitHelper.Dao.Interfaces;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class PropertiesController : Controller
    {
        private readonly IDataSummitHelper _dataSummitHelper;
        
        public PropertiesController(IDataSummitHelper dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        DataSummitHelper.Properties propertiesService = new DataSummitHelper.Properties(new DataSummitDbContext());

        [HttpGet("drawing/{id}")]
        public async Task<IActionResult> GetDrawingProperties([FromRoute] string id)
        {
            var drawingId = int.Parse(id);
            var drawingPropertyRows = await _dataSummitHelper.GetDrawingProperties(drawingId);

            var drawingProperties = drawingPropertyRows
                .Select(row => DrawingProperty.FromDto(row))
                .ToList();

            return Ok(drawingProperties);
        }

        [HttpPost]
        public async Task UpdateDrawingProperty([FromBody]DrawingProperty property)
        {
            await _dataSummitHelper.UpdateDrawingPropertyValue(property.Id, property.Value);
            propertiesService.UpdateProperty(property.Id, property.Value);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            DrawingData dd = propertiesService.GetAllDrawingProperties(id);
            return JsonConvert.SerializeObject(dd);
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