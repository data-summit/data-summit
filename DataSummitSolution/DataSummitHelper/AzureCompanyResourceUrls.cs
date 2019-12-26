using DataSummitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class AzureCompanyResourceUrls
    {
        private DataSummitDbContext dataSummitDbContext;

        public AzureCompanyResourceUrls(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.AzureCompanyResourceUrls> GetAllCompanyAzureCompanyResourceUrls(int companyId)
        {
            List<DataSummitModels.AzureCompanyResourceUrls> AzureCompanyResourceUrls = new List<DataSummitModels.AzureCompanyResourceUrls>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                AzureCompanyResourceUrls = dataSummitDbContext.AzureCompanyResourceUrls.Where(
                                    e => e.CompanyId == companyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return AzureCompanyResourceUrls;
        }

        public Uri GetIndividualUrl(int companyId, AzureResource azureResource)
        {
            Uri uri = null;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.AzureCompanyResourceUrls res = dataSummitDbContext.AzureCompanyResourceUrls.First(
                            e => e.CompanyId == companyId &&
                            e.Name == azureResource.ToString());
                uri = new Uri(res.Url + "?code=" + res.Key);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return uri;
        }

        public enum AzureResource
        {
            SplitDocument = 1,
            ImageToContainer = 2,
            DivideImage = 3,
            RecogniseTextAzure = 4,
            PostProcessing = 5
        }
    }
}