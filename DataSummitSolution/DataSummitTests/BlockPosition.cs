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
    public class BlockPositionTests
    {
        [TestMethod]
        public void Create_new_blockPosition()
        {
            DataSummitModels.DB.BlockPosition blockPosition = new DataSummitModels.DB.BlockPosition
            {
                Name = "Unit Test BlockPosition",
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var mockBlockPositionsDbSet = new Mock<DbSet<DataSummitModels.DB.BlockPosition>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.BlockPositions).Returns(mockBlockPositionsDbSet.Object);
            var mockBlockPositions = new DataSummitHelper.BlockPosition(mockContext.Object);

            mockBlockPositions.CreateBlockPosition(blockPosition);

            mockBlockPositionsDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.DB.BlockPosition>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_blockPosition_by_id()
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
            Company ompany2 = new Company
            {
                CompanyId = 2,
                Name = "Unit Test Company2",
                CompanyNumber = "0000002",
                Vatnumber = "00000002",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany2.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testBlockPositions = new List<DataSummitModels.DB.BlockPosition>
            {
                new DataSummitModels.DB.BlockPosition
                {
                    BlockPositionId = 1,
                    Name = "Unit Test BlockPosition1",
                    CreatedDate = DateTime.Now,
                },
                new DataSummitModels.DB.BlockPosition
                {
                    BlockPositionId = 2,
                    Name = "Unit Test BlockPosition2",
                    CreatedDate = DateTime.Now
                },
                new DataSummitModels.DB.BlockPosition
                {
                    BlockPositionId = 3,
                    Name = "Unit Test BlockPosition3",
                    CreatedDate = DateTime.Now
                }
            }.AsQueryable();

            var mockBlockPositionDbSet = new Mock<DbSet<DataSummitModels.DB.BlockPosition>>();
            mockBlockPositionDbSet.As<IQueryable<DataSummitModels.DB.BlockPosition>>().Setup(m => m.Provider).Returns(testBlockPositions.Provider);
            mockBlockPositionDbSet.As<IQueryable<DataSummitModels.DB.BlockPosition>>().Setup(m => m.Expression).Returns(testBlockPositions.Expression);
            mockBlockPositionDbSet.As<IQueryable<DataSummitModels.DB.BlockPosition>>().Setup(m => m.ElementType).Returns(testBlockPositions.ElementType);
            mockBlockPositionDbSet.As<IQueryable<DataSummitModels.DB.BlockPosition>>().Setup(m => m.GetEnumerator()).Returns(testBlockPositions.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.BlockPositions).Returns(mockBlockPositionDbSet.Object);

            var mockBlockPositionService = new DataSummitHelper.BlockPosition(mockContext.Object);
            var mockBlockPosition = mockBlockPositionService.GetAllCompanyBlockPositions(1);
            Assert.AreEqual(mockBlockPosition.FirstOrDefault().BlockPositionId, testBlockPositions.ToList()[0].BlockPositionId);
        }
    }
}

