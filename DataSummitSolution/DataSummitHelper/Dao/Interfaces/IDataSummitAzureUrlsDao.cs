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
        Task<Tuple<string, string>> GetAzureUrlByName(string name);
        #endregion
    }
}
