using DataSummitModels.DB;
using DataSummitWeb.DTO;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProfileVersionsController : Controller
    {
        DataSummitHelper.ProfileVersions profileVersionsService = new DataSummitHelper.ProfileVersions(new DataSummitDbContext());
        private DataSummitDbContext db = new DataSummitDbContext();

        // GET api/profileVersion/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            ProfileVersions profileVersions = profileVersionsService.GetProfileVersion(id);

            var jss = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
            return JsonConvert.SerializeObject(profileVersions, Formatting.Indented, jss);
        }

        // GET api/profileVersion/5
        [HttpGet("{companyid}/profileversions/{id}")]
        public string Get(int companyid, int id)
        {
            List<ProfileVersions> lProfileVersions = profileVersionsService
                                                        .GetAllCompanyProfileVersions(companyid);
            return JsonConvert.SerializeObject(lProfileVersions.ToArray());
        }

        // POST api/profileVersion
        [HttpPost]
        //public string Post([FromBody]ProfileVersion profileversion)
        public string Post([FromBody] ProfileVersionDTO profileVersion)
        {
            int id = 0;
            try
            {
                DataSummitModels.DB.ProfileVersions pv = null;
                List<DataSummitModels.DB.ProfileAttributes> lPa = new List<DataSummitModels.DB.ProfileAttributes>();
                pv = new DataSummitModels.DB.ProfileVersions
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
                    DataSummitModels.DB.ProfileAttributes npa = new DataSummitModels.DB.ProfileAttributes
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
                id = profileVersionsService.CreateProfileVersion(pv);
            }
            catch (System.Exception ae)
            { string strError = ae.Message.ToString(); }

            //Upload and map
            return JsonConvert.SerializeObject(id);
        }

        // PUT api/profileVersion/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]ProfileVersions profileVersion)
        {
            //Update
            profileVersionsService.UpdateProfileVersion(id, profileVersion);
            return;
        }

        // DELETE api/profileVersion/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            profileVersionsService.DeleteProfileVersion(id);
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