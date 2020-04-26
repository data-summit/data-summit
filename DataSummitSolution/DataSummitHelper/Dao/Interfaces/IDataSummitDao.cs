using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Context;
using DsDb = DataSummitModels.DB;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitDao
    {
        Task<List<DsDb.ProfileAttributes>> GetProfileAttributesForDrawingId(int drawingId);
    }
}
