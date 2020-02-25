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
    public class StandardAttributeTests
    {
        [TestMethod]
        public void Create_new_standardAttribute()
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
            DataSummitModels.DB.StandardAttributes standardAttribute = new DataSummitModels.DB.StandardAttributes
            {
                StandardAttributeId = 1,
                Name = "Unit Test StandardAttribute",
                ProfileAttributes = new List<DataSummitModels.DB.ProfileAttributes> { profileAttribute }
            };

            var mockStandardAttributesDbSet = new Mock<DbSet<DataSummitModels.DB.StandardAttributes>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.StandardAttributes).Returns(mockStandardAttributesDbSet.Object);
            var mockStandardAttributes = new DataSummitHelper.StandardAttributes(mockContext.Object);

            mockStandardAttributes.CreateStandardAttribute(standardAttribute);

            mockStandardAttributesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.StandardAttributes>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_standardAttribute_by_id()
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
            DataSummitModels.DB.StandardAttributes standardAttribute = new DataSummitModels.DB.StandardAttributes
            {
                StandardAttributeId = 1,
                Name = "Unit Test StandardAttribute",
                ProfileAttributes = new List<DataSummitModels.DB.ProfileAttributes> { profileAttribute }
            };

            var testStandardAttributes = new List<DataSummitModels.DB.StandardAttributes>
            {
                new DataSummitModels.DB.StandardAttributes
                {
                    StandardAttributeId = 1,
                    Name = "Unit Test StandardAttribute1",
                    ProfileAttributes = new List<DataSummitModels.DB.ProfileAttributes> { profileAttribute }
                },
                new DataSummitModels.DB.StandardAttributes
                {
                    StandardAttributeId = 2,
                    Name = "Unit Test StandardAttribute2",
                    ProfileAttributes = new List<DataSummitModels.DB.ProfileAttributes> { profileAttribute }
                },
                new DataSummitModels.DB.StandardAttributes
                {
                    StandardAttributeId = 3,
                    Name = "Unit Test StandardAttribute3",
                    ProfileAttributes = new List<DataSummitModels.DB.ProfileAttributes> { profileAttribute }
                }
            }.AsQueryable();

            var mockStandardAttributeDbSet = new Mock<DbSet<DataSummitModels.DB.StandardAttributes>>();
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttributes>>().Setup(m => m.Provider).Returns(testStandardAttributes.Provider);
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttributes>>().Setup(m => m.Expression).Returns(testStandardAttributes.Expression);
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttributes>>().Setup(m => m.ElementType).Returns(testStandardAttributes.ElementType);
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttributes>>().Setup(m => m.GetEnumerator()).Returns(testStandardAttributes.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.StandardAttributes).Returns(mockStandardAttributeDbSet.Object);

            var mockStandardAttributeService = new DataSummitHelper.StandardAttributes(mockContext.Object);
            var mockStandardAttribute = mockStandardAttributeService.GetStandardAttributesById(1);
            Assert.AreEqual(mockStandardAttribute.StandardAttributeId, testStandardAttributes.ToList()[0].StandardAttributeId);
        }
    }
}

