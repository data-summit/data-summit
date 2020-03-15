using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Addresses
    {
        private DataSummitDbContext dataSummitDbContext;

        public Addresses(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.Addresses> GetAllCompanyAddresses(int CompanyId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.DB.Addresses> Addresses = new List<DataSummitModels.DB.Addresses>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                Addresses = dataSummitDbContext.Addresses.Where(e => e.CompanyId == CompanyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Addresses;
        }

        public List<DataSummitModels.DB.Addresses> GetAllProjectAddresses(int ProjectId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.DB.Addresses> Addresses = new List<DataSummitModels.DB.Addresses>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                Addresses = dataSummitDbContext.Addresses.Where(e => e.ProjectId == ProjectId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Addresses;
        }

        public long CreateAddress(DataSummitModels.DB.Addresses Address, bool IsProdEnvironment = false)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
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

        public void UpdateAddress(int id, DataSummitModels.DB.Addresses Address, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
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
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.Addresses Address = dataSummitDbContext.Addresses.First(p => p.AddressId == AddressId);
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
