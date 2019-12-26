using DataSummitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Projects
    {
        private DataSummitDbContext dataSummitDbContext;

        public Projects(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.Projects> GetAllCompanyProjects(int companyId)
        {
            List<DataSummitModels.Projects> Projects = new List<DataSummitModels.Projects>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                Projects = dataSummitDbContext.Projects.Where(e => e.CompanyId == companyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Projects;
        }

        public List<DataSummitModels.Projects> GetAllProjectsTemplates(int companyId, int projectId)
        {
            List<DataSummitModels.Projects> Projects = new List<DataSummitModels.Projects>();
            //try
            //{
            //    if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
            //    Projects = dataSummitDbContext.Projects.Where(e => e.CompanyId == companyId).ToList();
            //}
            //catch (Exception ae)
            //{
            //    string strError = ae.Message.ToString();
            //}
            return Projects;
        }

        public long CreateProject(DataSummitModels.Projects project)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Projects.Add(project);
                dataSummitDbContext.SaveChanges();
                returnid = project.ProjectId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }

        public void UpdateProject(int id, DataSummitModels.Projects project)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Projects.Update(project);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteProject(int projectId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.Projects Projects = dataSummitDbContext.Projects.First(p => p.ProjectId == projectId);
                dataSummitDbContext.Projects.Remove(Projects);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
