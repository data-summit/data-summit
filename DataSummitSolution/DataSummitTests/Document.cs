using DataSummitHelper;
using DataSummitModels.DB;
using DataSummitModels.DTO;
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
            List<DataSummitModels.DB.Document> ldocuments = new List<DataSummitModels.DB.Document>();
            DataSummitModels.DB.Document document = new DataSummitModels.DB.Document
            {
                DocumentId = 1,
                FileName = "Unit Test Document1",
                AmazonConfidence = 1,
                BlobUrl = "1",
                GoogleConfidence = (decimal)0.1,
                AzureConfidence = (decimal)0.1,
                Processed = true,
                TemplateVersionId = 1,
                ProjectId = 1,
                Success = true,
                Type = 2.ToString(), //DataSummitModels.Enums.Document.Type.DrawingPlanView,
                Tasks = new List<Task>() { new FunctionTask("Upload duration", DateTime.Now) },
                CreatedDate = DateTime.Now,
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            ldocuments.Add(document);

            var mockDocumentsDbSet = new Mock<DbSet<DataSummitModels.DB.Document>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Documents).Returns(mockDocumentsDbSet.Object);
            var mockDocuments = new DataSummitHelper.Document(mockContext.Object);

            mockDocuments.CreateDocument(ldocuments);

            mockDocumentsDbSet.Verify((DbSet<DataSummitModels.DB.Document> m) => m.Add(It.IsAny<DataSummitModels.DB.Document>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_document_by_id()
        {
            DataSummitModels.DB.Project project1 = new DataSummitModels.DB.Project
            {
                ProjectId = 1,
                Name = "Unit Test Project1",
                CompanyId = 1,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.DB.Project project2 = new DataSummitModels.DB.Project
            {
                ProjectId = 2,
                Name = "Unit Test Project2",
                CompanyId = 2,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testDocuments = new List<DataSummitModels.DB.Document>
            {
                new DataSummitModels.DB.Document
                {
                    DocumentId = 1,
                    FileName = "Unit Test Document1",
                    AmazonConfidence = 1,
                    BlobUrl = "1",
                    GoogleConfidence = (decimal)0.1,
                    AzureConfidence = (decimal)0.1,
                    Processed = true,
                    TemplateVersionId = 1,
                    Project = project1,
                    ProjectId = 1,
                    Success = true,
                    Type = 2.ToString(), //DataSummitModels.Enums.Document.Type.DrawingPlanView,
                    Tasks = new List<Task>() {new FunctionTask("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.Document
                {
                    DocumentId = 2,
                    FileName = "Unit Test Document2",
                    AmazonConfidence = 2,
                    BlobUrl = "2",
                    GoogleConfidence = (decimal)0.2,
                    AzureConfidence = (decimal)0.2,
                    Processed = true,
                    TemplateVersionId = 2,
                    Project = project2,
                    ProjectId = 2,
                    Success = true,
                    Type = 2.ToString(), //DataSummitModels.Enums.Document.Type.DrawingPlanView,
                    Tasks = new List<Task>() {new FunctionTask("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.DB.Document
                {
                    DocumentId = 3,
                    FileName = "Unit Test Document3",
                    AmazonConfidence = 3,
                    BlobUrl = "3",
                    GoogleConfidence = (decimal)0.3,
                    AzureConfidence = (decimal)0.3,
                    Processed = true,
                    TemplateVersionId = 3,
                    Project = project1,
                    ProjectId = 3,
                    Success = true,
                    Type = 2.ToString(), //DataSummitModels.Enums.Document.Type.DrawingPlanView,
                    Tasks = new List<Task>() {new FunctionTask("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockDocumentDbSet = new Mock<DbSet<DataSummitModels.DB.Document>>();
            mockDocumentDbSet.As<IQueryable<DataSummitModels.DB.Document>>().Setup((IQueryable<DataSummitModels.DB.Document> m) => m.Provider).Returns(testDocuments.Provider);
            mockDocumentDbSet.As<IQueryable<DataSummitModels.DB.Document>>().Setup((IQueryable<DataSummitModels.DB.Document> m) => m.Expression).Returns(testDocuments.Expression);
            mockDocumentDbSet.As<IQueryable<DataSummitModels.DB.Document>>().Setup((IQueryable<DataSummitModels.DB.Document> m) => m.ElementType).Returns(testDocuments.ElementType);
            mockDocumentDbSet.As<IQueryable<DataSummitModels.DB.Document>>().Setup((IQueryable<DataSummitModels.DB.Document> m) => m.GetEnumerator()).Returns(testDocuments.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Documents).Returns(mockDocumentDbSet.Object);

            var mockDocumentService = new DataSummitHelper.Document(mockContext.Object);
            var mockDocument = mockDocumentService.GetAllCompanyDocuments(1);
            Assert.AreEqual(mockDocument.FirstOrDefault().DocumentId, testDocuments.ToList()[0].DocumentId);
        }
    }
}

