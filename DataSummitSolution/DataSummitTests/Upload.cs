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
    public class DocumentUploadTests
    {
        [TestMethod]
        public void Create_new_drawing()
        {
            List<DocumentUpload> ldrawings = new List<DocumentUpload>();
            DocumentUpload drawing = new DocumentUpload();
            

            ldrawings.Add(drawing);

            var mockDocumentUploadsDbSet = new Mock<DbSet<DocumentUpload>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment


            //var mockContext = new Mock<DataSummitDbContext>(false);
            //mockContext.Setup(m => m.DocumentUpload).Returns(mockDocumentUploadsDbSet.Object);
            //var mockDocumentUploads = new DocumentUpload(mockContext.Object);

            //mockDocumentUploads.(ldrawings);

            //mockDocumentUploadsDbSet.Verify((DbSet<DocumentUpload> m) => m.Add(It.IsAny<DocumentUpload>()), Times.Once());
            //mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}

