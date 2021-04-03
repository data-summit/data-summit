using DataSummitService.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IDataSummitTemplateAttributesService
    {
        Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId);
    }
}