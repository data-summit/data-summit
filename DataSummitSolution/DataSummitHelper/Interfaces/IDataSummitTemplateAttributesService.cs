using DataSummitHelper.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitTemplateAttributesService
    {
        Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId);
    }
}