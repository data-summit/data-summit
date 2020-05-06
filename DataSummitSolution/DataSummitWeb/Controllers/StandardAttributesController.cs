using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataSummitModels.DB;

namespace DataSummitWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardAttributesController : ControllerBase
    {
        DataSummitHelper.StandardAttribute standardAttributesService = new DataSummitHelper.StandardAttribute(new DataSummitDbContext());
        private DataSummitDbContext db = new DataSummitDbContext();

        // GET: api/StandardAttributes/5
        [HttpGet("{id}")]
        public string GetStandardAttributes(short id)
        {
            List<StandardAttributes> lStandardAttributes = standardAttributesService
                                                        .GetAllCompanyStandardAttributes(id);
            return JsonConvert.SerializeObject(lStandardAttributes.ToArray());
        }

        // PUT: api/StandardAttributes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStandardAttributes(short id, StandardAttributes standardAttributes)
        {
            if (id != standardAttributes.StandardAttributeId)
            {
                return BadRequest();
            }

            db.Entry(standardAttributes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StandardAttributesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //// POST: api/StandardAttributes
        //[HttpPost]
        //public async Task<ActionResult<StandardAttributes>> PostStandardAttributes(StandardAttributes standardAttributes)
        //{
        //    db.StandardAttributes.Add(standardAttributes);
        //    await db.SaveChangesAsync();

        //    return CreatedAtAction("GetStandardAttributes", new { id = standardAttributes.StandardAttributeId }, standardAttributes);
        //}

        [HttpPost]
        public string PostStandardAttributes(ProfileVersions profileVersion)
        {
            return JsonConvert.SerializeObject(profileVersion);
        }

        // DELETE: api/StandardAttributes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StandardAttributes>> DeleteStandardAttributes(short id)
        {
            var standardAttributes = await db.StandardAttributes.FindAsync(id);
            if (standardAttributes == null)
            {
                return NotFound();
            }

            db.StandardAttributes.Remove(standardAttributes);
            await db.SaveChangesAsync();

            return standardAttributes;
        }

        private bool StandardAttributesExists(short id)
        {
            return db.StandardAttributes.Any(e => e.StandardAttributeId == id);
        }
    }
}
