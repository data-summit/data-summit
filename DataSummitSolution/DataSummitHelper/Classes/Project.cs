using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class Project
    {
        private DataSummitDbContext dataSummitDbContext;

        public Project(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<Projects> GetAllCompanyProjects(int companyId)
        {
            List<Projects> Projects = new List<Projects>();
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

        public List<Projects> GetAllProjectsTemplates(int companyId, int projectId)
        {
            List<Projects> Projects = new List<Projects>();
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

        public long CreateProject(Projects project)
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

        public void UpdateProject(int id, Projects project)
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
                Projects Projects = dataSummitDbContext.Projects.First(p => p.ProjectId == projectId);
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
