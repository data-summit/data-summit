using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Context;
using DataSummitHelper.Dao.Interfaces;
using DataSummitModels.DB;
using Microsoft.EntityFrameworkCore;
using DsDb = DataSummitModels.DB;

namespace DataSummitHelper.Dao
{
    public class DataSummitDao : IDataSummitDao
    {
        private readonly DataSummitDbContext _context;

        public DataSummitDao(DataSummitDbContext context)
        {
            _context = context;

            // TODO: Guard class against null objects
            if (_context == null)
            {
                throw new Exception("DataSummit DbContext could not be created");
            }
        }

        public async Task<List<DsDb.ProfileAttributes>> GetProfileAttributesForDrawingId(int drawingId)
        {
            var profileAttributes = new List<DsDb.ProfileAttributes>();
            
            try
            {
                var drawing = await _context.Drawings.FirstOrDefaultAsync(d => d.DrawingId == drawingId);
                profileAttributes = _context.ProfileVersions.FirstOrDefault(p => p.ProfileVersionId == drawing.ProfileVersionId)
                    .ProfileAttributes
                    .ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }

            return profileAttributes;
        }
    }
}
