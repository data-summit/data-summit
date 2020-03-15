using DataSummitHelper;
using DataSummitModels.DB;
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
    public class ProfileAttributeTests
    {
        [TestMethod]
        public void Create_new_profileAttribute()
        {
            DataSummitModels.DB.ProfileAttributes profileAttribute = new DataSummitModels.DB.ProfileAttributes
            {
                ProfileAttributeId = 1,
                Name = "Unit Test ProfileAttribute1",
                BlockPositionId = 1,
                NameHeight = 100,
                NameWidth = 1000,
                NameX = 1000,
                NameY = 100,
                PaperSizeId = 1,
                ProfileVersionId = 1,
                Value = "10",
                ValueHeight = 100,
                ValueWidth = 1000,
                ValueX = 1000,
                ValueY = 100,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var mockProfileAttributesDbSet = new Mock<DbSet<DataSummitModels.DB.ProfileAttributes>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.ProfileAttributes).Returns(mockProfileAttributesDbSet.Object);
            var mockProfileAttributes = new DataSummitHelper.ProfileAttributes(mockContext.Object);

            mockProfileAttributes.CreateProfileAttribute(profileAttribute);

            mockProfileAttributesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.ProfileAttributes>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_profileAttribute_by_id()
        {
            DataSummitModels.DB.ProfileVersions profileVersion1 = new DataSummitModels.DB.ProfileVersions
            {
                ProfileVersionId = 1,
                Name = "Unit Test ProfileVersion1",
                CreatedDate = DateTime.Now,
                Height = 100,
                HeightOriginal = 1000,
                CompanyId = 1,
                Width = 100,
                WidthOriginal = 1000,
                X = 100,
                Y = 100
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.DB.ProfileVersions profileVersion2 = new DataSummitModels.DB.ProfileVersions
            {
                ProfileVersionId = 2,
                Name = "Unit Test ProfileVersion2",
                CreatedDate = DateTime.Now,
                Height = 200,
                HeightOriginal = 2000,
                CompanyId = 2,
                Width = 200,
                WidthOriginal = 2000,
                X = 200,
                Y = 200
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testProfileAttributes = new List<DataSummitModels.DB.ProfileAttributes>
            {
                new DataSummitModels.DB.ProfileAttributes
                {
                    ProfileAttributeId = 1,
                    Name = "Unit Test ProfileAttribute1",
                    BlockPositionId = 1,
                    NameHeight = 100,
                    NameWidth = 1000,
                    NameX = 1000,
                    NameY = 100,
                    PaperSizeId = 1,
                    ProfileVersion = profileVersion1,
                    ProfileVersionId = profileVersion1.ProfileVersionId,
                    Value = "10",
                    ValueHeight = 100, 
                    ValueWidth = 1000,
                    ValueX = 1000,
                    ValueY = 100,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.ProfileAttributes
                {
                    ProfileAttributeId = 2,
                    Name = "Unit Test ProfileAttribute2",
                    BlockPositionId = 2,
                    NameHeight = 200,
                    NameWidth = 2000,
                    NameX = 2000,
                    NameY = 200,
                    PaperSizeId = 2,
                    ProfileVersion = profileVersion2,
                    ProfileVersionId = profileVersion2.ProfileVersionId,
                    Value = "20",
                    ValueHeight = 200,
                    ValueWidth = 2000,
                    ValueX = 2000,
                    ValueY = 200,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.ProfileAttributes
                {
                    ProfileAttributeId = 3,
                    Name = "Unit Test ProfileAttribute3",
                    BlockPositionId = 3,
                    NameHeight = 300,
                    NameWidth = 3000,
                    NameX = 3000,
                    NameY = 300,
                    PaperSizeId = 3,
                    ProfileVersion = profileVersion1,
                    ProfileVersionId = profileVersion1.ProfileVersionId,
                    Value = "30",
                    ValueHeight = 300,
                    ValueWidth = 3000,
                    ValueX = 3000,
                    ValueY = 300,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockProfileAttributeDbSet = new Mock<DbSet<DataSummitModels.DB.ProfileAttributes>>();
            mockProfileAttributeDbSet.As<IQueryable<DataSummitModels.DB.ProfileAttributes>>().Setup(m => m.Provider).Returns(testProfileAttributes.Provider);
            mockProfileAttributeDbSet.As<IQueryable<DataSummitModels.DB.ProfileAttributes>>().Setup(m => m.Expression).Returns(testProfileAttributes.Expression);
            mockProfileAttributeDbSet.As<IQueryable<DataSummitModels.DB.ProfileAttributes>>().Setup(m => m.ElementType).Returns(testProfileAttributes.ElementType);
            mockProfileAttributeDbSet.As<IQueryable<DataSummitModels.DB.ProfileAttributes>>().Setup(m => m.GetEnumerator()).Returns(testProfileAttributes.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.ProfileAttributes).Returns(mockProfileAttributeDbSet.Object);

            var mockProfileAttributeService = new DataSummitHelper.ProfileAttributes(mockContext.Object);
            var mockProfileAttribute = mockProfileAttributeService.GetProfileAttributesById(1);
            Assert.AreEqual(mockProfileAttribute.ProfileAttributeId, testProfileAttributes.ToList()[0].ProfileAttributeId);
        }
    }
}

