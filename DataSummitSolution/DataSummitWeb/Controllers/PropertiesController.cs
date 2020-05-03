using DataSummitWeb.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using DataSummitHelper.Interfaces;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class PropertiesController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;
        
        public PropertiesController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        // GET api/properties/drawing/{id}
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

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDrawingPropertyValue([FromBody] DrawingPropertyValue drawingPropertyValue)
        {
            await _dataSummitHelper.UpdateDrawingPropertyValue(drawingPropertyValue.ToDto());
            return Ok();
        }

        // DELETE api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _dataSummitHelper.DeleteDrawingProperty(id);
            return Ok();
        }
    }
}