using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Companies
    {
        DataSummitDbContext dataSummitDbContext;

        public Companies(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.Companies> GetAllCompanies()
        {
            List<DataSummitModels.DB.Companies> companies = new List<DataSummitModels.DB.Companies>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                companies = dataSummitDbContext.Companies.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return companies;
        }

        public DataSummitModels.DB.Companies GetCompanyById(int companyId)
        {
            DataSummitModels.DB.Companies company = new DataSummitModels.DB.Companies();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                company = dataSummitDbContext.Companies.FirstOrDefault(c => c.CompanyId == companyId);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return company;
        }

        public int CreateCompany(DataSummitModels.DB.Companies company)
        {
            int returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                company.CreatedDate = DateTime.Now;
                dataSummitDbContext.Companies.Add(company);
                dataSummitDbContext.SaveChanges();
                returnid = company.CompanyId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }

        public void UpdateCompany(DataSummitModels.DB.Companies company)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Companies.Update(company);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteCompany(int companyId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.Companies company = dataSummitDbContext.Companies.First(p => p.CompanyId == companyId);
                dataSummitDbContext.Companies.Remove(company);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
