using DataSummitService.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Dao.Interfaces
{
    public interface IDataSummitAzureUrlsDao
    {
        #region Azure URLs
        Task<Tuple<string, string>> GetAzureFunctionUrlByName(string name);
        #endregion

        #region Machine Learning URLs
        Task<AzureMLResource> GetAzureMLResourceByNameAsync(string name);
        #endregion
    }
}
