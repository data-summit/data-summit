using DataSummitModels.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper.Classes
{
    public class Company
    {
        DataSummitDbContext dataSummitDbContext;

        public Company(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public async Task<List<DataSummitModels.DB.Company>> GetAllCompanies()
        {
            List<DataSummitModels.DB.Company> companies = new List<DataSummitModels.DB.Company>();
            try
            {
                companies = await dataSummitDbContext.Companies.ToListAsync();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return companies;
        }

        public DataSummitModels.DB.Company GetCompanyById(int companyId)
        {
            DataSummitModels.DB.Company company = new DataSummitModels.DB.Company();
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

        public int CreateCompany(DataSummitModels.DB.Company company)
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

        public void UpdateCompany(DataSummitModels.DB.Company company)
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
                DataSummitModels.DB.Company company = dataSummitDbContext.Companies.First(p => p.CompanyId == companyId);
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
