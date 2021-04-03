using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitMachineLearningDao
    {
        #region ML URLs
        Task<AzureMLResource> GetMLUrlByName(string name);
        #endregion
    }
}
