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
    public class TemplateVersionsController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;

        public TemplateVersionsController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        // GET api/templateVersion/company/5
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompanyTemplates(int id)
        {
            var templateVersions = await _dataSummitHelper.GetAllCompanyTemplates(id);

            return Ok(templateVersions);
        }

        // GET api/templateVersion/project/5
        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetProjectTemplates(int id)
        {
            var templates = await _dataSummitHelper.GetAllProjectTemplates(id);

            return Ok(templates);
        }

        // POST api/templateVersion
        [HttpPost]
        public string Post([FromBody] TemplateVersionDTO templateVersion)
        {
            int id = 0;
            try
            {
                TemplateVersion pv = null;
                List<TemplateAttribute> lPa = new List<TemplateAttribute>();
                pv = new TemplateVersion
                {
                    CompanyId = templateVersion.CompanyId,
                    Name = templateVersion.Name,
                    CreatedDate = DateTime.Now,
                    HeightOriginal = (int)templateVersion.Height,
                    WidthOriginal = (int)templateVersion.Width,
                    Height = (int)templateVersion.Height,
                    Width = (int)templateVersion.Width,
                    //TODO this needs to updated with actual logged in user id
                    UserId = 1
                };
                System.Threading.Tasks.Task t = System.Threading.Tasks.Task.Run(() =>
                {
                    string i = templateVersion.ImageString.Replace("data:image/jpeg;base64,", "");
                    pv.Image = Convert.FromBase64String(i);
                });
                
                foreach (TemplateAttribute pa in templateVersion.TemplateAttributes)
                {
                    TemplateAttribute npa = new TemplateAttribute
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
                pv.TemplateAttributes = lPa;
                t.Wait();
                //id = templateVersionsService.CreateTemplateVersion(pv);
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }

            //Upload and map
            return JsonConvert.SerializeObject(id);
        }

        // PUT api/templateVersion/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]TemplateVersion templateVersion)
        {
            //Update
            //templateVersionsService.UpdateTemplateVersion(id, templateVersion);
            return;
        }

        // DELETE api/templateVersion/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //templateVersionsService.DeleteTemplateVersion(id);
            return JsonConvert.SerializeObject("Ok");
        }

        private async Task<byte[]> ConvertImage(string image)
        {
            byte[] array = null;
            try
            {
                System.Threading.Tasks.Task t = System.Threading.Tasks.Task.Run(() =>
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