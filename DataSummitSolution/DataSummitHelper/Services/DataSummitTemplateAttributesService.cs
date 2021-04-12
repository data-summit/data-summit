using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;

namespace DataSummitService.Services
{
    public class DataSummitTemplateAttributesService : IDataSummitTemplateAttributesService
    {
        private readonly IDataSummitTemplateAttributesDao _dao;

        public DataSummitTemplateAttributesService(IDataSummitTemplateAttributesDao dao)
        {
            _dao = dao;
        }

        public async Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId)
        {
            var templateAttributes = await _dao.GetAttribtesForTemplateId(templateId);
            var templateAttributeDtos = templateAttributes.Select(d => new TemplateAttributeDto(d))
                .ToList();

            return templateAttributeDtos;
        }
    }
}