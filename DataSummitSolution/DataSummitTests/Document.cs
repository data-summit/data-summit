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
    public class DocumentTests
    {
        [TestMethod]
        public void Create_new_document()
        {
            List<Documents> ldocuments = new List<Documents>();
            Documents document = new Documents
            {
                DocumentId = 1,
                FileName = "Unit Test Document1",
                AmazonConfidence = 1,
                BlobUrl = "1",
                GoogleConfidence = (decimal)0.1,
                AzureConfidence = (decimal)0.1,
                Processed = true,
                ProfileVersionId = 1,
                ProjectId = 1,
                Success = true,
                Type = DataSummitModels.Enums.Document.Type.DrawingPlanView,
                Tasks = new List<Tasks>() { new Tasks("Upload duration", DateTime.Now) },
                CreatedDate = DateTime.Now,
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            ldocuments.Add(document);

            var mockDocumentsDbSet = new Mock<DbSet<Documents>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Documents).Returns(mockDocumentsDbSet.Object);
            var mockDocuments = new Document(mockContext.Object);

            mockDocuments.CreateDocument(ldocuments);

            mockDocumentsDbSet.Verify((DbSet<Documents> m) => m.Add(It.IsAny<Documents>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_document_by_id()
        {
            Projects project1 = new Projects
            {
                ProjectId = 1,
                Name = "Unit Test Project1",
                CompanyId = 1,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            Projects project2 = new Projects
            {
                ProjectId = 2,
                Name = "Unit Test Project2",
                CompanyId = 2,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testDocuments = new List<Documents>
            {
                new Documents
                {
                    DocumentId = 1,
                    FileName = "Unit Test Document1",
                    AmazonConfidence = 1,
                    BlobUrl = "1",
                    GoogleConfidence = (decimal)0.1,
                    AzureConfidence = (decimal)0.1,
                    Processed = true,
                    ProfileVersionId = 1,
                    Project = project1,
                    ProjectId = 1,
                    Success = true,
                    Type = DataSummitModels.Enums.Document.Type.DrawingPlanView,
                    Tasks = new List<Tasks>() {new Tasks("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new Documents
                {
                    DocumentId = 2,
                    FileName = "Unit Test Document2",
                    AmazonConfidence = 2,
                    BlobUrl = "2",
                    GoogleConfidence = (decimal)0.2,
                    AzureConfidence = (decimal)0.2,
                    Processed = true,
                    ProfileVersionId = 2,
                    Project = project2,
                    ProjectId = 2,
                    Success = true,
                    Type = DataSummitModels.Enums.Document.Type.DrawingPlanView,
                    Tasks = new List<Tasks>() {new Tasks("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new Documents
                {
                    DocumentId = 3,
                    FileName = "Unit Test Document3",
                    AmazonConfidence = 3,
                    BlobUrl = "3",
                    GoogleConfidence = (decimal)0.3,
                    AzureConfidence = (decimal)0.3,
                    Processed = true,
                    ProfileVersionId = 3,
                    Project = project1,
                    ProjectId = 3,
                    Success = true,
                    Type = DataSummitModels.Enums.Document.Type.DrawingPlanView,
                    Tasks = new List<Tasks>() {new Tasks("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockDocumentDbSet = new Mock<DbSet<Documents>>();
            mockDocumentDbSet.As<IQueryable<Documents>>().Setup((IQueryable<Documents> m) => m.Provider).Returns(testDocuments.Provider);
            mockDocumentDbSet.As<IQueryable<Documents>>().Setup((IQueryable<Documents> m) => m.Expression).Returns(testDocuments.Expression);
            mockDocumentDbSet.As<IQueryable<Documents>>().Setup((IQueryable<Documents> m) => m.ElementType).Returns(testDocuments.ElementType);
            mockDocumentDbSet.As<IQueryable<Documents>>().Setup((IQueryable<Documents> m) => m.GetEnumerator()).Returns(testDocuments.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Documents).Returns(mockDocumentDbSet.Object);

            var mockDocumentService = new Document(mockContext.Object);
            var mockDocument = mockDocumentService.GetAllCompanyDocuments(1);
            Assert.AreEqual(mockDocument.FirstOrDefault().DocumentId, testDocuments.ToList()[0].DocumentId);
        }
    }
}

