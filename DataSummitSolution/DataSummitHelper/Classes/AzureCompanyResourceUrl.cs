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

        public List<DataSummitModels.DB.AzureCompanyResourceUrl> GetAllCompanyAzureCompanyResourceUrl(int companyId)
        {
            List<DataSummitModels.DB.AzureCompanyResourceUrl> AzureCompanyResourceUrl = new List<DataSummitModels.DB.AzureCompanyResourceUrl>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                AzureCompanyResourceUrl = dataSummitDbContext.AzureCompanyResourceUrls.Where(
                                    e => e.CompanyId == companyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return AzureCompanyResourceUrl;
        }

        public Uri GetIndividualUrl(int companyId, AzureResource azureResource)
        {
            Uri uri = null;
            try
            {
                DataSummitModels.DB.AzureCompanyResourceUrl res = dataSummitDbContext.AzureCompanyResourceUrls.First(
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