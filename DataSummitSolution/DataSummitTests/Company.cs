using DataSummitHelper;
using DataSummitModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitTests
{
    [TestClass]
    public class CompanyTest
    {
        /// <summary>
        /// Can we create a new company?
        /// </summary>
        [TestMethod]
        public void Create_new_company()
        {
            DataSummitModels.Companies company = new DataSummitModels.Companies
            {
                Name = "Unit Test Company",
                CompanyNumber = "0000000",
                Vatnumber = "00000000",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var mockCompanyDbSet = new Mock<DbSet<DataSummitModels.Companies>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Companies).Returns(mockCompanyDbSet.Object);
            var mockCompany = new DataSummitHelper.Companies(mockContext.Object);

            mockCompany.CreateCompany(company);

            mockCompanyDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.Companies>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_company_by_id()
        {
            var testCompanies = new List<DataSummitModels.Companies>
            {
                new DataSummitModels.Companies
                {
                    CompanyId = 1,
                    Name = "Unit Test Company1",
                    CompanyNumber = "0000001",
                    Vatnumber = "00000001",
                    CreatedDate = DateTime.Now,
                    Website = "www.UnitTestCompany1.com"
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Companies
                {
                    CompanyId = 2,
                    Name = "Unit Test Company2",
                    CompanyNumber = "0000002",
                    Vatnumber = "00000002",
                    CreatedDate = DateTime.Now,
                    Website = "www.UnitTestCompany2.com"
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Companies
                {
                    CompanyId = 3,
                    Name = "Unit Test Company3",
                    CompanyNumber = "0000003",
                    Vatnumber = "00000003",
                    CreatedDate = DateTime.Now,
                    Website = "www.UnitTestCompany3.com"
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockCompanyDbSet = new Mock<DbSet<DataSummitModels.Companies>>();
            mockCompanyDbSet.As<IQueryable<DataSummitModels.Companies>>().Setup(m => m.Provider).Returns(testCompanies.Provider);
            mockCompanyDbSet.As<IQueryable<DataSummitModels.Companies>>().Setup(m => m.Expression).Returns(testCompanies.Expression);
            mockCompanyDbSet.As<IQueryable<DataSummitModels.Companies>>().Setup(m => m.ElementType).Returns(testCompanies.ElementType);
            mockCompanyDbSet.As<IQueryable<DataSummitModels.Companies>>().Setup(m => m.GetEnumerator()).Returns(testCompanies.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Companies).Returns(mockCompanyDbSet.Object);

            var mockCompanyService = new DataSummitHelper.Companies(mockContext.Object);
            var mockCompany = mockCompanyService.GetCompanyById(1);
            Assert.AreEqual(mockCompany.CompanyNumber, testCompanies.ToList()[0].CompanyNumber);
        }
    }
}