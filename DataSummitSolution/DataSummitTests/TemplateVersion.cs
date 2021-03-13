using DataSummitHelper;
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
    public class TemplateVersionTests
    {
        [TestMethod]
        public void Create_new_templateVersion()
        {
            TemplateVersions templateVersion = new TemplateVersions
            {
                TemplateVersionId = 1,
                Height = 100,
                HeightOriginal = 1000,
                Name = "00001",
                CompanyId = 1,
                Width = 1000,
                WidthOriginal = 10000,
                X = 1000,
                Y = 100,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var mockTemplateVersionsDbSet = new Mock<DbSet<TemplateVersions>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.TemplateVersions).Returns(mockTemplateVersionsDbSet.Object);
            var mockTemplateVersions = new TemplateVersion(mockContext.Object);

            mockTemplateVersions.CreateTemplateVersion(templateVersion);

            mockTemplateVersionsDbSet.Verify(m => m.Add(It.IsAny<TemplateVersions>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_templateVersion_by_id()
        {
            var testTemplateVersions = new List<TemplateVersions>
            {
                new TemplateVersions
                {
                    TemplateVersionId = 1,
                    Height = 100,
                    HeightOriginal = 1000,
                    Name = "00001",
                    CompanyId = 1,
                    Width = 1000,
                    WidthOriginal = 10000,
                    X = 1000,
                    Y = 100,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new TemplateVersions
                {
                    TemplateVersionId = 2,
                    Height = 200,
                    HeightOriginal = 2000,
                    Name = "00002",
                    CompanyId = 2,
                    Width = 2000,
                    WidthOriginal = 20000,
                    X = 2000,
                    Y = 200,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new TemplateVersions
                {
                    TemplateVersionId = 3,
                    Height = 300,
                    HeightOriginal = 3000,
                    Name = "00003",
                    CompanyId = 3,
                    Width = 3000,
                    WidthOriginal = 30000,
                    X = 3000,
                    Y = 300,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockTemplateVersionDbSet = new Mock<DbSet<TemplateVersions>>();
            mockTemplateVersionDbSet.As<IQueryable<TemplateVersions>>().Setup(m => m.Provider).Returns(testTemplateVersions.Provider);
            mockTemplateVersionDbSet.As<IQueryable<TemplateVersions>>().Setup(m => m.Expression).Returns(testTemplateVersions.Expression);
            mockTemplateVersionDbSet.As<IQueryable<TemplateVersions>>().Setup(m => m.ElementType).Returns(testTemplateVersions.ElementType);
            mockTemplateVersionDbSet.As<IQueryable<TemplateVersions>>().Setup(m => m.GetEnumerator()).Returns(testTemplateVersions.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.TemplateVersions).Returns(mockTemplateVersionDbSet.Object);

            var mockTemplateVersionService = new TemplateVersion(mockContext.Object);
            var mockTemplateVersion = mockTemplateVersionService.GetAllCompanyTemplateVersions(1);
            Assert.AreEqual(mockTemplateVersion.FirstOrDefault().TemplateVersionId, testTemplateVersions.ToList()[0].TemplateVersionId);
        }
    }
}

