
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;

namespace DataSummitHelper.Services 
{
    public class DataSummitHelperService : IDataSummitHelper
    {
        private readonly IDataSummitDao _dao;

        public DataSummitHelperService(IDataSummitDao dao)
        {
            _dao = dao;
        }

        public async Task<List<DrawingPropertyDto>> GetDrawingProperties(int drawingId)
        {
            var profileAttributes = await _dao.GetProfileAttributesForDrawingId(drawingId);

            var drawingProperties = profileAttributes.Select(pa => new DrawingPropertyDto(pa))
                .ToList();

            return drawingProperties;
        }

        public Task UpdateDrawingPropertyValue(int drawingPropertyId, string drawingPropertyValue)
        {
            throw new System.NotImplementedException();
        }
    }
}