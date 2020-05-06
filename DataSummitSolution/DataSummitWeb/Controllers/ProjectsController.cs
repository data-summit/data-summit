using DataSummitHelper.Interfaces;
using DataSummitWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ProjectsController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        // GET api/projects/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var projectDtos = await _dataSummitHelper.GetAllCompanyProjects(id);
            var projects = projectDtos.Select(p => Project.FromDto(p))
                .ToList();
            return Ok(projects);
        }

        // POST api/projects/create
        [HttpPost("create")]
        public async Task CreateProject([FromBody]Project project)
        {
            await _dataSummitHelper.CreateProject(project.ToDto());
        }

        // PUT api/projects/update
        [HttpPut("update")]
        public async Task UpdateProject([FromBody]Project project)
        {
            //Update
            await _dataSummitHelper.UpdateProject(project.ToDto());
        }

        // DELETE api/projects/delete/{id}
        [HttpDelete("delete")]
        public async Task DeleteProjectById([FromBody]int id)
        {
            await _dataSummitHelper.DeleteProject(id);
        }
    }
}
