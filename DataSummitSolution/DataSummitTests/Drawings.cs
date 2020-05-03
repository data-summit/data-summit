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
    public class DrawingTests
    {
        [TestMethod]
        public void Create_new_drawing()
        {
            List<Drawings> ldrawings = new List<Drawings>();
            Drawings drawing = new Drawings
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

            var mockDrawingsDbSet = new Mock<DbSet<Drawings>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Drawings).Returns(mockDrawingsDbSet.Object);
            var mockDrawings = new Drawing(mockContext.Object);

            mockDrawings.CreateDrawing(ldrawings);

            mockDrawingsDbSet.Verify((DbSet<Drawings> m) => m.Add(It.IsAny<Drawings>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_drawing_by_id()
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

            var testDrawings = new List<Drawings>
            {
                new Drawings
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
                new Drawings
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
                new Drawings
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

            var mockDrawingDbSet = new Mock<DbSet<Drawings>>();
            mockDrawingDbSet.As<IQueryable<Drawings>>().Setup((IQueryable<Drawings> m) => m.Provider).Returns(testDrawings.Provider);
            mockDrawingDbSet.As<IQueryable<Drawings>>().Setup((IQueryable<Drawings> m) => m.Expression).Returns(testDrawings.Expression);
            mockDrawingDbSet.As<IQueryable<Drawings>>().Setup((IQueryable<Drawings> m) => m.ElementType).Returns(testDrawings.ElementType);
            mockDrawingDbSet.As<IQueryable<Drawings>>().Setup((IQueryable<Drawings> m) => m.GetEnumerator()).Returns(testDrawings.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Drawings).Returns(mockDrawingDbSet.Object);

            var mockDrawingService = new Drawing(mockContext.Object);
            var mockDrawing = mockDrawingService.GetAllCompanyDrawings(1);
            Assert.AreEqual(mockDrawing.FirstOrDefault().DrawingId, testDrawings.ToList()[0].DrawingId);
        }
    }
}

