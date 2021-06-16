using DataSummitService.Classes;
using DataSummitService.Dao.Interfaces;
using DataSummitModels.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitService.Dto;

namespace DataSummitService.Dao
{
    public partial class DataSummitAzureDao : IDataSummitAzureUrlsDao
    {
        private readonly DataSummitDbContext _context;

        public DataSummitAzureDao(DataSummitDbContext context)
        {
            _context = context;

            // TODO: Guard class against null objects
            if (_context == null)
            {
                throw new Exception("DataSummit DbContext could not be created");
            }
            // TODO: Guard class against empty objects (TJ amended, can this TODO be removed now?)
            else if (_context.AzureCompanyResourceUrls?.Any() ?? false)
            {
                throw new Exception("DataSummit DbContext contains no results");
            }
        }

        public async Task DeleteTemplateAttribute(long templateAttributeId)
        {
            try
            {
                var templateAttribute = new DataSummitModels.DB.TemplateAttribute()
                { TemplateAttributeId = templateAttributeId };
                _context.TemplateAttributes.Attach(templateAttribute);
                _context.TemplateAttributes.Remove(templateAttribute);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Azure URLs
        public async Task<AzureFunctionUrlKeyDto> GetAzureFunctionUrlByName(string name)
        {
            var urlKey = await _context.AzureCompanyResourceUrls.SingleAsync(ar => ar.Name == name);
            return new AzureFunctionUrlKeyDto
            {
                Url = urlKey.Url,
                Key = urlKey.Key
            };
        }
        #endregion

        #region ML URLs
        public async Task<AzureMLResource> GetAzureMLResourceByNameAsync(string name)
        {
            var azML = await _context.AzureMLResources.SingleAsync(ar => ar.Name == name);
            return azML;
        }

        public AzureMLResource GetMLUrlByName(string name)
        {
            var azML = _context.AzureMLResources.SingleOrDefault(ar => ar.Name == name);
            return azML;
        }
        #endregion

        public async Task<List<DataSummitModels.DB.TemplateAttribute>> GetAttribtesForTemplateId(int templateId)
        {
            var templateAttributes = new List<DataSummitModels.DB.TemplateAttribute>();

            templateAttributes = await _context.TemplateAttributes
                .Where(p => p.TemplateVersionId == templateId)
                .ToListAsync();

            return templateAttributes;
        }
    }
}
