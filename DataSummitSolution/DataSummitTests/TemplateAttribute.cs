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
    public class TemplateAttributeTests
    {
        [TestMethod]
        public void Create_new_templateAttribute()
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

            var mockTemplateAttributesDbSet = new Mock<DbSet<DataSummitModels.DB.TemplateAttribute>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.TemplateAttributes).Returns(mockTemplateAttributesDbSet.Object);
            var mockTemplateAttributes = new DataSummitHelper.TemplateAttribute(mockContext.Object);

            mockTemplateAttributes.CreateTemplateAttribute(templateAttribute);

            mockTemplateAttributesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.TemplateAttribute>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_templateAttribute_by_id()
        {
            DataSummitModels.DB.TemplateVersion templateVersion1 = new DataSummitModels.DB.TemplateVersion
            {
                TemplateVersionId = 1,
                Name = "Unit Test TemplateVersion1",
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
            DataSummitModels.DB.TemplateVersion templateVersion2 = new DataSummitModels.DB.TemplateVersion
            {
                TemplateVersionId = 2,
                Name = "Unit Test TemplateVersion2",
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

            var testTemplateAttributes = new List<DataSummitModels.DB.TemplateAttribute>
            {
                new DataSummitModels.DB.TemplateAttribute
                {
                    TemplateAttributeId = 1,
                    Name = "Unit Test TemplateAttribute1",
                    BlockPositionId = 1,
                    NameHeight = 100,
                    NameWidth = 1000,
                    NameX = 1000,
                    NameY = 100,
                    PaperSizeId = 1,
                    TemplateVersion = templateVersion1,
                    TemplateVersionId = templateVersion1.TemplateVersionId,
                    Value = "10",
                    ValueHeight = 100, 
                    ValueWidth = 1000,
                    ValueX = 1000,
                    ValueY = 100,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.TemplateAttribute
                {
                    TemplateAttributeId = 2,
                    Name = "Unit Test TemplateAttribute2",
                    BlockPositionId = 2,
                    NameHeight = 200,
                    NameWidth = 2000,
                    NameX = 2000,
                    NameY = 200,
                    PaperSizeId = 2,
                    TemplateVersion = templateVersion2,
                    TemplateVersionId = templateVersion2.TemplateVersionId,
                    Value = "20",
                    ValueHeight = 200,
                    ValueWidth = 2000,
                    ValueX = 2000,
                    ValueY = 200,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.TemplateAttribute
                {
                    TemplateAttributeId = 3,
                    Name = "Unit Test TemplateAttribute3",
                    BlockPositionId = 3,
                    NameHeight = 300,
                    NameWidth = 3000,
                    NameX = 3000,
                    NameY = 300,
                    PaperSizeId = 3,
                    TemplateVersion = templateVersion1,
                    TemplateVersionId = templateVersion1.TemplateVersionId,
                    Value = "30",
                    ValueHeight = 300,
                    ValueWidth = 3000,
                    ValueX = 3000,
                    ValueY = 300,
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockTemplateAttributeDbSet = new Mock<DbSet<DataSummitModels.DB.TemplateAttribute>>();
            mockTemplateAttributeDbSet.As<IQueryable<DataSummitModels.DB.TemplateAttribute>>().Setup(m => m.Provider).Returns(testTemplateAttributes.Provider);
            mockTemplateAttributeDbSet.As<IQueryable<DataSummitModels.DB.TemplateAttribute>>().Setup(m => m.Expression).Returns(testTemplateAttributes.Expression);
            mockTemplateAttributeDbSet.As<IQueryable<DataSummitModels.DB.TemplateAttribute>>().Setup(m => m.ElementType).Returns(testTemplateAttributes.ElementType);
            mockTemplateAttributeDbSet.As<IQueryable<DataSummitModels.DB.TemplateAttribute>>().Setup(m => m.GetEnumerator()).Returns(testTemplateAttributes.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.TemplateAttributes).Returns(mockTemplateAttributeDbSet.Object);

            var mockTemplateAttributeService = new DataSummitHelper.TemplateAttribute(mockContext.Object);
            var mockTemplateAttribute = mockTemplateAttributeService.GetTemplateAttributesById(1);
            Assert.AreEqual(mockTemplateAttribute.TemplateAttributeId, testTemplateAttributes.ToList()[0].TemplateAttributeId);
        }
    }
}

