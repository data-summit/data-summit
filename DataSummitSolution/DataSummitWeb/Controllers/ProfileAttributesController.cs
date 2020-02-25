using DataSummitHelper;
using DataSummitModels.DB;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProfileAttributesController : Controller
    {
        DataSummitHelper.ProfileAttributes profileAttributeService = new DataSummitHelper.ProfileAttributes(new DataSummitDbContext());
        DataSummitHelper.BlockPositions blockPositionService = new DataSummitHelper.BlockPositions(new DataSummitDbContext());
        DataSummitHelper.StandardAttributes standardAttributeService = new DataSummitHelper.StandardAttributes(new DataSummitDbContext());
        DataSummitHelper.PaperSizes paperSizeSerice = new DataSummitHelper.PaperSizes(new DataSummitDbContext());

        private DataSummitDbContext db = new DataSummitDbContext();
        // GET api/profileAttributes/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            List<DataSummitModels.DB.ProfileAttributes> lProfileAttributes = profileAttributeService.GetAllProfileVersionProfileAttributes(id);
            try
            {                                                 
                foreach (DataSummitModels.DB.ProfileAttributes pa in lProfileAttributes)
                {
                    pa.BlockPosition = blockPositionService.GetBlockPositionById(pa.BlockPositionId);
                    pa.PaperSize = paperSizeSerice.GetPaperSizesById(pa.PaperSizeId);
                    pa.StandardAttribute = standardAttributeService.GetStandardAttributesById((short)pa.StandardAttributeId);
                }
            }
            catch (System.Exception ae)
            {
                string strError = "";
                if (ae.Message != null) strError = ae.Message.ToString();
            }
            return JsonConvert.SerializeObject(lProfileAttributes.ToArray());
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]DataSummitModels.DB.ProfileAttributes profileAttribute)
        {
            //Create
            return JsonConvert.SerializeObject(
                profileAttributeService.CreateProfileAttribute(profileAttribute));
        }

        // PUT api/profileAttributes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DataSummitModels.DB.ProfileAttributes profileAttribute)
        {
            //Update
            profileAttributeService.UpdateProfileAttribute(id, profileAttribute);
            return;
        }

        // DELETE api/profileAttributes/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            profileAttributeService.DeleteProfileAttribute(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}