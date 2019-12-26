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
    public class PropertyTests
    {
        [TestMethod]
        public void Create_new_property()
        {
            DataSummitModels.Properties property = new DataSummitModels.Properties
            {
                PropertyId = 1,
                ProfileAttributeId = 1,
                SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            var mockPropertiesDbSet = new Mock<DbSet<DataSummitModels.Properties>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Properties).Returns(mockPropertiesDbSet.Object);
            var mockProperties = new DataSummitHelper.Properties(mockContext.Object);

            mockProperties.CreateProperty(property);

            mockPropertiesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.Properties>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_property_by_id()
        {

            var testProperties = new List<DataSummitModels.Properties>
            {
                new DataSummitModels.Properties
                {
                    PropertyId = 1,
                    ProfileAttributeId = 1,
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Sentence = new DataSummitModels.Sentences
                    {
                        DrawingId = 1
                    }
                },
                new DataSummitModels.Properties
                {
                    PropertyId = 2,
                    ProfileAttributeId = 2,
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Sentence = new DataSummitModels.Sentences
                    {
                        DrawingId = 1
                    }
                },
                new DataSummitModels.Properties
                {
                    PropertyId = 3,
                    ProfileAttributeId = 3,
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Sentence = new DataSummitModels.Sentences
                    {
                        DrawingId = 1
                    }
                }
            }.AsQueryable();

            var mockPropertyDbSet = new Mock<DbSet<DataSummitModels.Properties>>();
            mockPropertyDbSet.As<IQueryable<DataSummitModels.Properties>>().Setup(m => m.Provider).Returns(testProperties.Provider);
            mockPropertyDbSet.As<IQueryable<DataSummitModels.Properties>>().Setup(m => m.Expression).Returns(testProperties.Expression);
            mockPropertyDbSet.As<IQueryable<DataSummitModels.Properties>>().Setup(m => m.ElementType).Returns(testProperties.ElementType);
            mockPropertyDbSet.As<IQueryable<DataSummitModels.Properties>>().Setup(m => m.GetEnumerator()).Returns(testProperties.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Properties).Returns(mockPropertyDbSet.Object);

            var mockPropertyService = new DataSummitHelper.Properties(mockContext.Object);
            var mockProperty = mockPropertyService.GetAllDrawingProperties(1);
            Assert.AreEqual(mockProperty.FirstOrDefault().PropertyId, testProperties.ToList()[0].PropertyId);
        }
    }
}

