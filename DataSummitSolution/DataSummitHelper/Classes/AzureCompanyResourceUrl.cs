using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class AzureCompanyResourceUrl
    {
        private DataSummitDbContext dataSummitDbContext;

        public AzureCompanyResourceUrl(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<AzureCompanyResourceUrls> GetAllCompanyAzureCompanyResourceUrls(int companyId)
        {
            List<AzureCompanyResourceUrls> AzureCompanyResourceUrls = new List<AzureCompanyResourceUrls>();
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
                AzureCompanyResourceUrls res = dataSummitDbContext.AzureCompanyResourceUrls.First(
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
            PostProcessing = 5,
            ExtractTitleBlock = 6
        }
    }
}