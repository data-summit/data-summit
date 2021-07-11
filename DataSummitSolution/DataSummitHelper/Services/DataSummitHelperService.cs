
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;
using DataSummitModels.DB;
using Microsoft.Extensions.Configuration;

namespace DataSummitService.Services
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

            var httpContent = (HttpContent)new StringContent(payload, Encoding.UTF8, "application/json");

            var stringTask = await client.PostAsync(uri, httpContent);
            return stringTask;
        }

        public async Task<HttpResponseMessage> ProcessCall(Uri uri, string payload, Dictionary<string, string> headers)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var httpContent = (HttpContent)new StringContent(payload, Encoding.UTF8, "application/json");

            var stringTask = await client.PostAsync(uri, httpContent);
            return stringTask;
        }
    }
}