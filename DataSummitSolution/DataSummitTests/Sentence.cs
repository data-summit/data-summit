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
    public class SentenceTests
    {
        [TestMethod]
        public void Create_new_Sentence()
        {
            Sentences Sentence = new Sentences
            {
                SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Confidence = (decimal)0.1,
                DocumentId = 1,
                //RectangleId = Guid.Parse("00000000-0000-0000-0000-100000000000"),
                IsUsed = true,
                Vendor = "Test",
                Words = "0001"
            };

            var mockSentenceDbSet = new Mock<DbSet<Sentences>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Sentences).Returns(mockSentenceDbSet.Object);
            var mockSentence = new Sentence(mockContext.Object);

            mockSentence.CreateSentence(Sentence);

            mockSentenceDbSet.Verify(m => m.Add(It.IsAny<Sentences>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_Sentence_by_id()
        {
            //DocumentVTemplate documentVTemplate1 = new DocumentVTemplate
            //{
            //    DocumentId = 1,
            //    DrawVtempId = 1,
            //    ProfileAttributeId = 1,
            //    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            //};
            //DocumentVTemplate documentVTemplate2 = new DocumentVTemplate
            //{
            //    DocumentId = 2,
            //    DrawVtempId = 2,
            //    ProfileAttributeId = 2,
            //    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            //};

            var testSentence = new List<Sentences>
            {
                new Sentences
                {
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Confidence = (decimal)0.1,
                    DocumentId = 1,
                    //RectangleId = Guid.Parse("00000000-0000-0000-0000-100000000000"),
                    IsUsed = true,
                    Vendor = "Test",
                    Words = "0001"
                },
                new Sentences
                {
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Confidence = (decimal)0.2,
                    DocumentId = 2,
                    //RectangleId = Guid.Parse("00000000-0000-0000-0000-200000000000"),
                    IsUsed = true,
                    Vendor = "Test",
                    Words = "0002"
                },
                new Sentences
                {
                    SentenceId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Confidence = (decimal)0.3,
                    DocumentId = 3,
                    //RectangleId = Guid.Parse("00000000-0000-0000-0000-300000000000"),
                    IsUsed = true,
                    Vendor = "Test",
                    Words = "0003"
                }
            }.AsQueryable();

            var mockSentenceDbSet = new Mock<DbSet<Sentences>>();
            mockSentenceDbSet.As<IQueryable<Sentences>>().Setup(m => m.Provider).Returns(testSentence.Provider);
            mockSentenceDbSet.As<IQueryable<Sentences>>().Setup(m => m.Expression).Returns(testSentence.Expression);
            mockSentenceDbSet.As<IQueryable<Sentences>>().Setup(m => m.ElementType).Returns(testSentence.ElementType);
            mockSentenceDbSet.As<IQueryable<Sentences>>().Setup(m => m.GetEnumerator()).Returns(testSentence.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Sentences).Returns(mockSentenceDbSet.Object);

            var mockSentenceService = new Sentence(mockContext.Object);
            var mockSentence = mockSentenceService.GetAllDocumentSentences(1);
            Assert.AreEqual(mockSentence.FirstOrDefault().SentenceId, testSentence.ToList()[0].SentenceId);
        }
    }
}

