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
    public class AddressTests
    {
        [TestMethod]
        public void Create_new_Address()
        {
            DataSummitModels.Addresses Address = new DataSummitModels.Addresses
            {
                AddressId = 1,
                CompanyId = 1,
                NumberName = "1",
                Street = "Test 1 Unit A",
                Street2 = "Test 1 Industrial Estate",
                Street3 = "Test 1 Street",
                TownCity = "Test City",
                CountryId = 225,
                PostCode = "T3S T1",
                County = "Surrey",
                CreatedDate = DateTime.Now,
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var mockAddressesDbSet = new Mock<DbSet<DataSummitModels.Addresses>>();
            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(m => m.Addresses).Returns(mockAddressesDbSet.Object);
            var mockAddresses = new DataSummitHelper.Addresses(mockContext.Object);

            mockAddresses.CreateAddress(Address);

            mockAddressesDbSet.Verify(m => m.Add(It.IsAny<DataSummitModels.Addresses>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_Company_Address_by_id()
        {
            DataSummitModels.Companies company1 = new DataSummitModels.Companies
            {
                CompanyId = 1,
                Name = "Unit Test Company1",
                CompanyNumber = "0000001",
                Vatnumber = "00000001",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany1.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.Companies company2 = new DataSummitModels.Companies
            {
                CompanyId = 2,
                Name = "Unit Test Company2",
                CompanyNumber = "0000002",
                Vatnumber = "00000002",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany2.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testAddresses = new List<DataSummitModels.Addresses>
            {
                new DataSummitModels.Addresses
                {
                    AddressId = 1,
                    CompanyId = 1,
                    NumberName = "1",
                    Street = "Test 1 Unit A",
                    Street2 = "Test 1 Industrial Estate",
                    Street3 = "Test 1 Street",
                    TownCity = "Test City",
                    CountryId = 225,
                    PostCode = "T3S T1",
                    County = "Surrey",
                    CreatedDate = DateTime.Now,
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Addresses
                {
                    AddressId = 2,
                    CompanyId = 2,
                    NumberName = "2",
                    Street = "Test 2 Unit A",
                    Street2 = "Test 2 Industrial Estate",
                    Street3 = "Test 2 Street",
                    TownCity = "Test City",
                    CountryId = 225,
                    PostCode = "T3S T2",
                    County = "Surrey",
                    CreatedDate = DateTime.Now,
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Addresses
                {
                    AddressId = 3,
                    CompanyId = 3,
                    NumberName = "3",
                    Street = "Test 3 Unit A",
                    Street2 = "Test 3 Industrial Estate",
                    Street3 = "Test 3 Street",
                    TownCity = "Test City",
                    CountryId = 225,
                    PostCode = "T3S T3",
                    County = "Surrey",
                    CreatedDate = DateTime.Now,
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockAddressDbSet = new Mock<DbSet<DataSummitModels.Addresses>>();
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.Provider).Returns(testAddresses.Provider);
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.Expression).Returns(testAddresses.Expression);
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.ElementType).Returns(testAddresses.ElementType);
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.GetEnumerator()).Returns(testAddresses.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Addresses).Returns(mockAddressDbSet.Object);

            var mockAddresseservice = new DataSummitHelper.Addresses(mockContext.Object);
            var mockAddress = mockAddresseservice.GetAllCompanyAddresses(1);
            Assert.AreEqual(mockAddress.FirstOrDefault().AddressId, testAddresses.ToList()[0].AddressId);
        }

        [TestMethod]
        public void Get_Project_Address_by_id()
        {
            DataSummitModels.Companies company1 = new DataSummitModels.Companies
            {
                CompanyId = 1,
                Name = "Unit Test Company1",
                CompanyNumber = "0000001",
                Vatnumber = "00000001",
                CreatedDate = DateTime.Now,
                Website = "www.UnitTestCompany1.com"
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };
            DataSummitModels.Projects Project1 = new DataSummitModels.Projects
            {
                ProjectId = 1,
                Name = "Unit Test Project1",
                Company = company1,
                CompanyId = company1.CompanyId,
                CreatedDate = DateTime.Now
                //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
            };

            var testAddresses = new List<DataSummitModels.Addresses>
            {
                new DataSummitModels.Addresses
                {
                    AddressId = 1,
                    ProjectId = 1,
                    NumberName = "1",
                    Street = "Test 1 Unit A",
                    Street2 = "Test 1 Industrial Estate",
                    Street3 = "Test 1 Street",
                    TownCity = "Test City",
                    CountryId = 225,
                    PostCode = "T3S T1",
                    County = "Surrey",
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Addresses
                {
                    AddressId = 2,
                    ProjectId = 2,
                    NumberName = "2",
                    Street = "Test 2 Unit A",
                    Street2 = "Test 2 Industrial Estate",
                    Street3 = "Test 2 Street",
                    TownCity = "Test City",
                    CountryId = 225,
                    PostCode = "T3S T2",
                    County = "Surrey",
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                },
                new DataSummitModels.Addresses
                {
                    AddressId = 3,
                    ProjectId = 3,
                    NumberName = "3",
                    Street = "Test 3 Unit A",
                    Street2 = "Test 3 Industrial Estate",
                    Street3 = "Test 3 Street",
                    TownCity = "Test City",
                    CountryId = 225,
                    PostCode = "T3S T3",
                    County = "Surrey",
                    CreatedDate = DateTime.Now
                    //UserId = "160e488d-2288-413a-935e-d3e339f8dd80"
                }
            }.AsQueryable();

            var mockAddressDbSet = new Mock<DbSet<DataSummitModels.Addresses>>();
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.Provider).Returns(testAddresses.Provider);
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.Expression).Returns(testAddresses.Expression);
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.ElementType).Returns(testAddresses.ElementType);
            mockAddressDbSet.As<IQueryable<DataSummitModels.Addresses>>().Setup(m => m.GetEnumerator()).Returns(testAddresses.GetEnumerator());

            //Mock<DataSummitDbContext>(false) is required should a parameterless DbContext not exist
            //otherwise Mock<DataSummitDbContext>() is permissible
            //false = Is Production environment | true = Is development environment
            var mockContext = new Mock<DataSummitDbContext>(false);
            mockContext.Setup(c => c.Addresses).Returns(mockAddressDbSet.Object);

            var mockAddresseservice = new DataSummitHelper.Addresses(mockContext.Object);
            var mockAddress = mockAddresseservice.GetAllProjectAddresses(1);
            Assert.AreEqual(mockAddress.FirstOrDefault().AddressId, testAddresses.ToList()[0].AddressId);
        }
    }
}

