using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;

namespace DataSummitHelper.Services
{
    public class DataSummitTemplatesService : IDataSummitTemplatesService
    {
        private readonly IDataSummitDao _dao;

        public DataSummitTemplatesService(IDataSummitDao dao)
        {
            _dao = dao;
        }

        #region Templates
        public async Task<List<TemplateDto>> GetAllCompanyTemplates(int companyId)
        {
            var templates = await _dao.GetCompanyTemplateVersions(companyId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateDto>> GetAllProjectTemplates(int projectId)
        {
            var templates = await _dao.GetProjectTemplateVersions(projectId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId)
        {
            var templateAttributes = await _dao.GetAttribtesForTemplateId(templateId);
            var templateAttributeDtos = templateAttributes.Select(d => new TemplateAttributeDto(d))
                .ToList();

            return templateAttributeDtos;
        }
        #endregion
    }
}