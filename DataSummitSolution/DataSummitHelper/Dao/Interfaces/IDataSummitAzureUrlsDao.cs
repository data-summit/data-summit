using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataSummitService.Dto;

namespace DataSummitService.Dao.Interfaces
{
    public interface IDataSummitAzureUrlsDao
    {
        #region Azure URLs
        Task<AzureFunctionUrlKeyDto> GetAzureFunctionUrlByName(string name);
        #endregion

        #region Machine Learning URLs
        Task<AzureMLResource> GetAzureMLResourceByNameAsync(string name);
        #endregion
    }
}
