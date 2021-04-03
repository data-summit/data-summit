using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;

namespace DataSummitService.Services
{
    public class DataSummitTemplatesService : IDataSummitTemplatesService
    {
        private readonly IDataSummitTemplatesDao _templatesDao;
        private readonly IDataSummitTemplateAttributesDao _attributesDao;

        public DataSummitTemplatesService(IDataSummitTemplatesDao templatesDao, IDataSummitTemplateAttributesDao attributesDao)
        {
            _templatesDao = templatesDao;
            _attributesDao = attributesDao;
        }

        #region Templates
        public async Task<List<TemplateDto>> GetAllCompanyTemplates(int companyId)
        {
            var templates = await _templatesDao.GetCompanyTemplateVersions(companyId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateDto>> GetAllProjectTemplates(int projectId)
        {
            var templates = await _templatesDao.GetProjectTemplateVersions(projectId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId)
        {
            var templateAttributes = await _attributesDao.GetAttribtesForTemplateId(templateId);
            var templateAttributeDtos = templateAttributes.Select(d => new TemplateAttributeDto(d))
                .ToList();

            return templateAttributeDtos;
        }
        #endregion
    }
}