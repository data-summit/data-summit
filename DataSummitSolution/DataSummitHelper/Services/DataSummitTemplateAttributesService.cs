using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;

namespace DataSummitHelper.Services
{
    public class DataSummitTemplateAttributesService : IDataSummitTemplateAttributesService
    {
        private readonly IDataSummitDao _dao;

        public DataSummitTemplateAttributesService(IDataSummitDao dao)
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