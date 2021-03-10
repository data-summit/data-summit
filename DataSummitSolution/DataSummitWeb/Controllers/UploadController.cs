using DataSummitHelper.Interfaces;
using DataSummitHelper.Services;
using DataSummitWeb.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataSummitWeb.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class UploadController : Controller //ControllerBase
    {
        private readonly IUploadService _upload;

        public UploadController(IUploadService upload)
        {
            _upload = upload ?? throw new ArgumentNullException(nameof(upload));
        }

        // GET: api/<UploadController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UploadController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UploadController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        return BadRequest("Uploaded file is empty or null.");
                    }
                    else
                    {
                        await _upload.ProcessUploadedFileAsync(file);
                    }
                }
            }

            return Ok();
        }

        // PUT api/<UploadController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UploadController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
