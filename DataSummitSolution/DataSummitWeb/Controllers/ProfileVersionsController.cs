using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using DataSummitWeb.DTO;
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
    public class ProfileVersionsController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ProfileVersionsController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        // GET api/profileVersion/company/5
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompanyTemplates(int id)
        {
            var profileVersions = await _dataSummitHelper.GetAllCompanyTemplates(id);

            return Ok(profileVersions);
        }

        // GET api/profileVersion/project/5
        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetProjectTemplates(int id)
        {
            var templates = await _dataSummitHelper.GetAllProjectTemplates(id);

            return Ok(templates);
        }

        // POST api/profileVersion
        [HttpPost]
        public string Post([FromBody] ProfileVersionDTO profileVersion)
        {
            int id = 0;
            try
            {
                ProfileVersions pv = null;
                List<ProfileAttributes> lPa = new List<ProfileAttributes>();
                pv = new ProfileVersions
                {
                    CompanyId = profileVersion.CompanyId,
                    Name = profileVersion.Name,
                    CreatedDate = DateTime.Now,
                    HeightOriginal = (int)profileVersion.Height,
                    WidthOriginal = (int)profileVersion.Width,
                    Height = (int)profileVersion.Height,
                    Width = (int)profileVersion.Width,
                    //TODO this needs to updated with actual logged in user id
                    UserId = 1
                };
                Task t = Task.Factory.StartNew(() =>
                {
                    string i = profileVersion.ImageString.Replace("data:image/jpeg;base64,", "");
                    pv.Image = Convert.FromBase64String(i);
                });
                
                foreach (ProfileAttributes pa in profileVersion.ProfileAttributes)
                {
                    ProfileAttributes npa = new ProfileAttributes
                    {
                        BlockPositionId = pa.BlockPositionId,
                        CreatedDate = DateTime.Now,
                        Name = pa.Name,  //Need to add OCR for small images from other existing project
                        NameHeight = pa.NameHeight,
                        NameWidth = pa.NameWidth,
                        NameX = (short)pa.NameX,
                        NameY = (short)pa.NameY,
                        PaperSizeId = 1,
                        Value = pa.Value,
                        ValueHeight = (short)pa.ValueHeight,
                        ValueWidth = (short)pa.ValueWidth,
                        ValueX = (short)pa.ValueX,
                        ValueY = (short)pa.ValueY,
                        StandardAttributeId = pa.StandardAttributeId
                    };
                    lPa.Add(npa);
                }
                pv.ProfileAttributes = lPa;
                t.Wait();
                //id = profileVersionsService.CreateProfileVersion(pv);
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }

            //Upload and map
            return JsonConvert.SerializeObject(id);
        }

        // PUT api/profileVersion/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]ProfileVersions profileVersion)
        {
            //Update
            //profileVersionsService.UpdateProfileVersion(id, profileVersion);
            return;
        }

        // DELETE api/profileVersion/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //profileVersionsService.DeleteProfileVersion(id);
            return JsonConvert.SerializeObject("Ok");
        }

        private async Task<byte[]> ConvertImage(string image)
        {
            byte[] array = null;
            try
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    array = Convert.FromBase64String(image);
                });
                await t;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return array;
        }
    }
}