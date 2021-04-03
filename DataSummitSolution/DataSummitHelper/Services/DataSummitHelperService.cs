
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using Microsoft.Extensions.Configuration;

namespace DataSummitHelper.Services
{
    public class DataSummitHelperService : IDataSummitHelperService
    {
        private readonly IConfiguration _configuration;

        public DataSummitHelperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Uri GetIndividualUrl(int companyId, string azureResource)
        {
            throw new NotImplementedException();
        }

        public string GetSecret(string secretName)
        {
            string key = "";
            try
            {
                key = _configuration[secretName];
            }
            catch (Exception)
            {
                return "";
            }
            return key;
        }

        public async Task<HttpResponseMessage> ProcessCall(Uri uri, string payload)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var stringTask = await client.PostAsync(uri, httpContent);
            return stringTask;
        }
    }
}