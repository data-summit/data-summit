using DataSummitHelper;
using DataSummitModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataSummitTests
{
    [TestClass]
    public class ProfileVersionTests
    {
        [TestMethod]
        public void Create_new_profileVersion()
        {
            DataSummitModels.ProfileVersions profileVersion = new DataSummitModels.ProfileVersions
            {
                ProfileVersionId = 1,
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

            var mockProfileVersionsDbSet = new Mock<DbSet<DataSummitModels.ProfileVersions>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.ProfileVersions).Returns(mockProfileVersionsDbSet.Object);
            var mockProfileVersions = new DataSummitHelper.ProfileVersions(mockContext.Object);

            mockProfileVersions.CreateProfileVersion(profileVersion);

            mockProfileVersionsDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.ProfileVersions>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_profileVersion_by_id()
        {
            var testProfileVersions = new List<DataSummitModels.ProfileVersions>
            {
                new DataSummitModels.ProfileVersions
                {
                    ProfileVersionId = 1,
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
                new DataSummitModels.ProfileVersions
                {
                    ProfileVersionId = 2,
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
                new DataSummitModels.ProfileVersions
                {
                    ProfileVersionId = 3,
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

            var mockProfileVersionDbSet = new Mock<DbSet<DataSummitModels.ProfileVersions>>();
            mockProfileVersionDbSet.As<IQueryable<DataSummitModels.ProfileVersions>>().Setup(m => m.Provider).Returns(testProfileVersions.Provider);
            mockProfileVersionDbSet.As<IQueryable<DataSummitModels.ProfileVersions>>().Setup(m => m.Expression).Returns(testProfileVersions.Expression);
            mockProfileVersionDbSet.As<IQueryable<DataSummitModels.ProfileVersions>>().Setup(m => m.ElementType).Returns(testProfileVersions.ElementType);
            mockProfileVersionDbSet.As<IQueryable<DataSummitModels.ProfileVersions>>().Setup(m => m.GetEnumerator()).Returns(testProfileVersions.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.ProfileVersions).Returns(mockProfileVersionDbSet.Object);

            var mockProfileVersionService = new DataSummitHelper.ProfileVersions(mockContext.Object);
            var mockProfileVersion = mockProfileVersionService.GetAllCompanyProfileVersions(1);
            Assert.AreEqual(mockProfileVersion.FirstOrDefault().ProfileVersionId, testProfileVersions.ToList()[0].ProfileVersionId);
        }
    }
}

