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
    public class StandardAttributeTests
    {
        [TestMethod]
        public void Create_new_standardAttribute()
        {
            DataSummitModels.DB.TemplateAttribute templateAttribute = new DataSummitModels.DB.TemplateAttribute
            {
                TemplateAttributeId = 1,
                Name = "Unit Test TemplateAttribute1",
                BlockPositionId = 1,
                NameHeight = 100,
                NameWidth = 1000,
                NameX = 1000,
                NameY = 100,
                PaperSizeId = 1,
                TemplateVersionId = 1,
                Value = "10",
                ValueHeight = 100,
                ValueWidth = 1000,
                ValueX = 1000,
                ValueY = 100,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.DB.StandardAttribute standardAttribute = new DataSummitModels.DB.StandardAttribute
            {
                StandardAttributeId = 1,
                Name = "Unit Test StandardAttribute",
                TemplateAttributes = new List<DataSummitModels.DB.TemplateAttribute> { templateAttribute }
            };

            var mockStandardAttributesDbSet = new Mock<DbSet<DataSummitModels.DB.StandardAttribute>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.StandardAttributes).Returns(mockStandardAttributesDbSet.Object);
            var mockStandardAttributes = new DataSummitHelper.StandardAttribute(mockContext.Object);

            mockStandardAttributes.CreateStandardAttribute(standardAttribute);

            mockStandardAttributesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.StandardAttribute>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_standardAttribute_by_id()
        {
            Company company1 = new Company
            {
                CompanyId = 1,
                Name = "Unit Test Company1",
                CompanyNumber = "0000001",
                Vatnumber = "00000001",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany1.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.DB.TemplateAttribute templateAttribute = new DataSummitModels.DB.TemplateAttribute
            {
                TemplateAttributeId = 1,
                Name = "Unit Test TemplateAttribute1",
                BlockPositionId = 1,
                NameHeight = 100,
                NameWidth = 1000,
                NameX = 1000,
                NameY = 100,
                PaperSizeId = 1,
                TemplateVersionId = 1,
                Value = "10",
                ValueHeight = 100,
                ValueWidth = 1000,
                ValueX = 1000,
                ValueY = 100,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.DB.StandardAttribute standardAttribute = new DataSummitModels.DB.StandardAttribute
            {
                StandardAttributeId = 1,
                Name = "Unit Test StandardAttribute",
                TemplateAttributes = new List<DataSummitModels.DB.TemplateAttribute> { templateAttribute }
            };

            var testStandardAttributes = new List<DataSummitModels.DB.StandardAttribute>
            {
                new DataSummitModels.DB.StandardAttribute
                {
                    StandardAttributeId = 1,
                    Name = "Unit Test StandardAttribute1",
                    TemplateAttributes = new List<DataSummitModels.DB.TemplateAttribute> { templateAttribute }
                },
                new DataSummitModels.DB.StandardAttribute
                {
                    StandardAttributeId = 2,
                    Name = "Unit Test StandardAttribute2",
                    TemplateAttributes = new List<DataSummitModels.DB.TemplateAttribute> { templateAttribute }
                },
                new DataSummitModels.DB.StandardAttribute
                {
                    StandardAttributeId = 3,
                    Name = "Unit Test StandardAttribute3",
                    TemplateAttributes = new List<DataSummitModels.DB.TemplateAttribute> { templateAttribute }
                }
            }.AsQueryable();

            var mockStandardAttributeDbSet = new Mock<DbSet<DataSummitModels.DB.StandardAttribute>>();
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttribute>>().Setup(m => m.Provider).Returns(testStandardAttributes.Provider);
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttribute>>().Setup(m => m.Expression).Returns(testStandardAttributes.Expression);
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttribute>>().Setup(m => m.ElementType).Returns(testStandardAttributes.ElementType);
            mockStandardAttributeDbSet.As<IQueryable<DataSummitModels.DB.StandardAttribute>>().Setup(m => m.GetEnumerator()).Returns(testStandardAttributes.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.StandardAttributes).Returns(mockStandardAttributeDbSet.Object);

            var mockStandardAttributeService = new DataSummitHelper.StandardAttribute(mockContext.Object);
            var mockStandardAttribute = mockStandardAttributeService.GetStandardAttributesById(1);
            Assert.AreEqual(mockStandardAttribute.StandardAttributeId, testStandardAttributes.ToList()[0].StandardAttributeId);
        }
    }
}

