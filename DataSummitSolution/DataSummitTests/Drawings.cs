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
    public class DrawingTests
    {
        [TestMethod]
        public void Create_new_drawing()
        {
            List<DataSummitModels.Drawings> ldrawings = new List<DataSummitModels.Drawings>();
            DataSummitModels.Drawings drawing = new DataSummitModels.Drawings
            {
                DrawingId = 1,
                FileName = "Unit Test Drawing1",
                AmazonConfidence = 1,
                BlobUrl = "1",
                GoogleConfidence = (decimal)0.1,
                AzureConfidence = (decimal)0.1,
                Processed = true,
                ProfileVersionId = 1,
                ProjectId = 1,
                Success = true,
                Type = "001",
                Tasks = new List<Tasks>() { new Tasks("Upload duration", DateTime.Now) },
                CreatedDate = DateTime.Now,
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            ldrawings.Add(drawing);

            var mockDrawingsDbSet = new Mock<DbSet<DataSummitModels.Drawings>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Drawings).Returns(mockDrawingsDbSet.Object);
            var mockDrawings = new DataSummitHelper.Drawings(mockContext.Object);

            mockDrawings.CreateDrawing(ldrawings);

            mockDrawingsDbSet.Verify((DbSet<DataSummitModels.Drawings> m) => m.Add(It.IsAny<DataSummitModels.Drawings>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_drawing_by_id()
        {
            DataSummitModels.Projects project1 = new DataSummitModels.Projects
            {
                ProjectId = 1,
                Name = "Unit Test Project1",
                CompanyId = 1,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.Projects project2 = new DataSummitModels.Projects
            {
                ProjectId = 2,
                Name = "Unit Test Project2",
                CompanyId = 2,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testDrawings = new List<DataSummitModels.Drawings>
            {
                new DataSummitModels.Drawings
                {
                    DrawingId = 1,
                    FileName = "Unit Test Drawing1",
                    AmazonConfidence = 1,
                    BlobUrl = "1",
                    GoogleConfidence = (decimal)0.1,
                    AzureConfidence = (decimal)0.1,
                    Processed = true,
                    ProfileVersionId = 1,
                    Project = project1,
                    ProjectId = 1,
                    Success = true,
                    Type = "001",
                    Tasks = new List<Tasks>() {new Tasks("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Drawings
                {
                    DrawingId = 2,
                    FileName = "Unit Test Drawing2",
                    AmazonConfidence = 2,
                    BlobUrl = "2",
                    GoogleConfidence = (decimal)0.2,
                    AzureConfidence = (decimal)0.2,
                    Processed = true,
                    ProfileVersionId = 2,
                    Project = project2,
                    ProjectId = 2,
                    Success = true,
                    Type = "002",
                    Tasks = new List<Tasks>() {new Tasks("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Drawings
                {
                    DrawingId = 3,
                    FileName = "Unit Test Drawing3",
                    AmazonConfidence = 3,
                    BlobUrl = "3",
                    GoogleConfidence = (decimal)0.3,
                    AzureConfidence = (decimal)0.3,
                    Processed = true,
                    ProfileVersionId = 3,
                    Project = project1,
                    ProjectId = 3,
                    Success = true,
                    Type = "003",
                    Tasks = new List<Tasks>() {new Tasks("Upload duration", DateTime.Now) },
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockDrawingDbSet = new Mock<DbSet<DataSummitModels.Drawings>>();
            mockDrawingDbSet.As<IQueryable<DataSummitModels.Drawings>>().Setup((IQueryable<DataSummitModels.Drawings> m) => m.Provider).Returns(testDrawings.Provider);
            mockDrawingDbSet.As<IQueryable<DataSummitModels.Drawings>>().Setup((IQueryable<DataSummitModels.Drawings> m) => m.Expression).Returns(testDrawings.Expression);
            mockDrawingDbSet.As<IQueryable<DataSummitModels.Drawings>>().Setup((IQueryable<DataSummitModels.Drawings> m) => m.ElementType).Returns(testDrawings.ElementType);
            mockDrawingDbSet.As<IQueryable<DataSummitModels.Drawings>>().Setup((IQueryable<DataSummitModels.Drawings> m) => m.GetEnumerator()).Returns(testDrawings.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Drawings).Returns(mockDrawingDbSet.Object);

            var mockDrawingService = new DataSummitHelper.Drawings(mockContext.Object);
            var mockDrawing = mockDrawingService.GetAllCompanyDrawings(1);
            Assert.AreEqual(mockDrawing.FirstOrDefault().DrawingId, testDrawings.ToList()[0].DrawingId);
        }
    }
}

