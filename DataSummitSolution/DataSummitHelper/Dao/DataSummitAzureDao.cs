using DataSummitService.Classes;
using DataSummitService.Dao.Interfaces;
using DataSummitModels.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            //// TODO: Guard class against empty objects
            //else if (_context.AzureCompanyResourceUrls.Count() > 0)
            //{
            //    throw new Exception("DataSummit DbContext contains no results");
            //}
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
        public async Task<Tuple<string, string>> GetAzureFunctionUrlByName(string name)
        {
            var urlKey = await _context.AzureCompanyResourceUrls.SingleOrDefaultAsync(ar => ar.Name == name);
            return new Tuple<string, string>(urlKey.Url, urlKey.Key);
        }
        #endregion

        #region ML URLs
        public async Task<AzureMLResource> GetMLUrlByNameAsync(string name)
        {
            var azML = await _context.AzureMLResources.SingleOrDefaultAsync(ar => ar.Name == name);
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
            try
            {
                templateAttributes = await _context.TemplateAttributes
                    .Where(p => p.TemplateVersionId == templateId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
            }

            return templateAttributes;
        }
    }
}
