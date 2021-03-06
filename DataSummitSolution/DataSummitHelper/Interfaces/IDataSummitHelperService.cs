using DataSummitService.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IDataSummitHelperService
    {
        Uri GetIndividualUrl(int companyId, string azureResource);

        #region Secrets
        string GetSecret(string secretName);
        #endregion

        Task<HttpResponseMessage> ProcessCall(Uri uri, string payload);
    }
}