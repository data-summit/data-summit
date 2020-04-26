using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataSummitHelper.Dto;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitHelper
    {
        Task<List<DrawingPropertyDto>> GetDrawingProperties(int drawingId);

        Task UpdateDrawingPropertyValue(int drawingPropertyId, string drawingPropertyValue);        
    }
}
