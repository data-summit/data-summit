using DataSummitService.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IDataSummitTemplatesService
    {

        Task<List<TemplateDto>> GetAllCompanyTemplates(int companyId);
        Task<List<TemplateDto>> GetAllProjectTemplates(int companyId);
        Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId);
    }
}