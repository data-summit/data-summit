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

        // GET api/properties/document/{id}
        [HttpGet("document/{id}")]
        public async Task<IActionResult> GetDocumentProperties([FromRoute] string id)
        {
            var documentId = int.Parse(id);
            var documentPropertyRows = await _dataSummitHelper.GetDocumentProperties(documentId);

            var documentProperties = documentPropertyRows
                .Select(row => DocumentProperty.FromDto(row))
                .ToList();

            return Ok(documentProperties);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDocumentPropertyValue([FromBody] DocumentPropertyValue documentPropertyValue)
        {
            await _dataSummitHelper.UpdateDocumentPropertyValue(documentPropertyValue.ToDto());
            return Ok();
        }

        // DELETE api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _dataSummitHelper.DeleteDocumentProperty(id);
            return Ok();
        }
    }
}