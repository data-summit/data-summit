using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitAzureUrlsDao
    {
        #region Azure URLs
        Task<Tuple<string, string>> GetAzureUrlByNameAsync(string name);
        Tuple<string, string> GetAzureUrlByName(string name);
        #endregion
    }
}
