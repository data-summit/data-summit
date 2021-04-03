using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class Address
    {
        private DataSummitModels.DB.DataSummitDbContext dataSummitDbContext;

        public Address(DataSummitModels.DB.DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.Address> GetAllCompanyAddresses(int CompanyId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.DB.Address> Addresses = new List<DataSummitModels.DB.Address>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                Addresses = dataSummitDbContext.Addresses.Where(e => e.CompanyId == CompanyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Addresses;
        }

        public List<DataSummitModels.DB.Address> GetAllProjectAddresses(int ProjectId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.DB.Address> Addresses = new List<DataSummitModels.DB.Address>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                Addresses = dataSummitDbContext.Addresses.Where(e => e.ProjectId == ProjectId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Addresses;
        }

        public long CreateAddress(DataSummitModels.DB.Address Address, bool IsProdEnvironment = false)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                Address.CreatedDate = DateTime.Now;
                dataSummitDbContext.Addresses.Add(Address);
                dataSummitDbContext.SaveChanges();
                returnid = Address.AddressId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }

        public void UpdateAddress(int id, DataSummitModels.DB.Address Address, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                dataSummitDbContext.Addresses.Update(Address);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteAddress(int AddressId, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                DataSummitModels.DB.Address Address = dataSummitDbContext.Addresses.First(p => p.AddressId == AddressId);
                dataSummitDbContext.Addresses.Remove(Address);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
