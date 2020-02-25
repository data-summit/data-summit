using DataSummitModels.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitTests
{
    [TestClass]
    public class PaperSizesTests
    {
        [TestMethod]
        public void Create_new_project()
        {
            DataSummitModels.DB.Projects project = new DataSummitModels.DB.Projects
            {
                Name = "Unit Test Project",
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var mockProjectsDbSet = new Mock<DbSet<DataSummitModels.DB.Projects>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Projects).Returns(mockProjectsDbSet.Object);
            var mockProjects = new DataSummitHelper.Projects(mockContext.Object);

            mockProjects.CreateProject(project);

            mockProjectsDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.Projects>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_project_by_id()
        {
            DataSummitModels.DB.Companies company1 = new DataSummitModels.DB.Companies
            {
                CompanyId = 1,
                Name = "Unit Test Company1",
                CompanyNumber = "0000001",
                Vatnumber = "00000001",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany1.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.DB.Companies company2 = new DataSummitModels.DB.Companies
            {
                CompanyId = 2,
                Name = "Unit Test Company2",
                CompanyNumber = "0000002",
                Vatnumber = "00000002",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany2.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testProjects = new List<DataSummitModels.DB.Projects>
            {
                new DataSummitModels.DB.Projects
                {
                    ProjectId = 1,
                    Name = "Unit Test Project1",
                    Company = company1,
                    CompanyId = company1.CompanyId,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.Projects
                {
                    ProjectId = 2,
                    Name = "Unit Test Project2",
                    Company = company2,
                    CompanyId = company2.CompanyId,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.Projects
                {
                    ProjectId = 3,
                    Name = "Unit Test Project3",
                    Company = company1,
                    CompanyId = company1.CompanyId,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockProjectDbSet = new Mock<DbSet<DataSummitModels.DB.Projects>>();
            mockProjectDbSet.As<IQueryable<DataSummitModels.DB.Projects>>().Setup(m => m.Provider).Returns(testProjects.Provider);
            mockProjectDbSet.As<IQueryable<DataSummitModels.DB.Projects>>().Setup(m => m.Expression).Returns(testProjects.Expression);
            mockProjectDbSet.As<IQueryable<DataSummitModels.DB.Projects>>().Setup(m => m.ElementType).Returns(testProjects.ElementType);
            mockProjectDbSet.As<IQueryable<DataSummitModels.DB.Projects>>().Setup(m => m.GetEnumerator()).Returns(testProjects.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Projects).Returns(mockProjectDbSet.Object);

            var mockProjectService = new DataSummitHelper.Projects(mockContext.Object);
            var mockProject = mockProjectService.GetAllCompanyProjects(1);
            Assert.AreEqual(mockProject.FirstOrDefault().ProjectId, testProjects.ToList()[0].ProjectId);
        }
    }
}

