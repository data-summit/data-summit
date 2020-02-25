using DataSummitHelper;
using DataSummitModels.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        //Connection string determined by Startup.IEnvironment and used privately in dbContext
        //Projects projectsService = new Projects(new DataSummitDbContext(Startup.ConnectionString));
        DataSummitHelper.Projects projectsService = new DataSummitHelper.Projects(new DataSummitDbContext());

        private DataSummitDbContext db = new DataSummitDbContext();
        // GET api/projects/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            List<DataSummitModels.DB.Projects> lProjects = projectsService.GetAllCompanyProjects(id);
            return JsonConvert.SerializeObject(lProjects.ToArray());
        }

        // POST api/projects
        [HttpPost]
        public string Post([FromBody]DataSummitModels.DB.Projects project)
        {
            //Create
            return JsonConvert.SerializeObject(projectsService.CreateProject(project));
        }

        // PUT api/projects/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DataSummitModels.DB.Projects project)
        {
            //Update
            projectsService.UpdateProject(id, project);
        }

        // DELETE api/projects/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            projectsService.DeleteProject(id);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}
