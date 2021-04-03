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
    public class PropertyTests
    {
        [TestMethod]
        public void Create_new_property()
        {
            DataSummitModels.DB.Property property = new DataSummitModels.DB.Property
            {
                PropertyId = 1,
                TemplateAttributeId = 1,
                SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            var mockPropertiesDbSet = new Mock<DbSet<DataSummitModels.DB.Property>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Properties).Returns(mockPropertiesDbSet.Object);
            var mockProperties = new DataSummitHelper.Property(mockContext.Object);

            mockProperties.CreateProperty(property);

            mockPropertiesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.Property>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_property_by_id()
        {
            var testProperties = new List<DataSummitModels.DB.Property>
            {
                new DataSummitModels.DB.Property
                {
                    PropertyId = 1,
                    TemplateAttributeId = 1,
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Sentence = new DataSummitModels.DB.Sentence
                    {
                        DocumentId = 1
                    }
                },
                new DataSummitModels.DB.Property
                {
                    PropertyId = 2,
                    TemplateAttributeId = 2,
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Sentence = new DataSummitModels.DB.Sentence
                    {
                        DocumentId = 1
                    }
                },
                new DataSummitModels.DB.Property
                {
                    PropertyId = 3,
                    TemplateAttributeId = 3,
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Sentence = new DataSummitModels.DB.Sentence
                    {
                        DocumentId = 1
                    }
                }
            }.AsQueryable();

            var mockPropertyDbSet = new Mock<DbSet<DataSummitModels.DB.Property>>();
            mockPropertyDbSet.As<IQueryable<DataSummitModels.DB.Property>>().Setup(m => m.Provider).Returns(testProperties.Provider);
            mockPropertyDbSet.As<IQueryable<DataSummitModels.DB.Property>>().Setup(m => m.Expression).Returns(testProperties.Expression);
            mockPropertyDbSet.As<IQueryable<DataSummitModels.DB.Property>>().Setup(m => m.ElementType).Returns(testProperties.ElementType);
            mockPropertyDbSet.As<IQueryable<DataSummitModels.DB.Property>>().Setup(m => m.GetEnumerator()).Returns(testProperties.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Properties).Returns(mockPropertyDbSet.Object);

            var mockPropertyService = new DataSummitHelper.Property(mockContext.Object);
            //TODO DB object changed for DTO object, test needs to be updated
            //Assert.AreEqual(mockProperty.FirstOrDefault(). .PropertyId, testProperties.ToList()[0].PropertyId);
        }
    }
}

