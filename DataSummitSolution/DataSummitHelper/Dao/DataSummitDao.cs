using DataSummitHelper.Classes;
using DataSummitHelper.Dao.Interfaces;
using DataSummitModels.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao
{
    public partial class DataSummitDao : IDataSummitDao
    {
        private readonly DataSummitDbContext _context;
        private readonly Company _company;

        public DataSummitDao(DataSummitDbContext context)
        {
            _context = context;
            _company = new Company(_context);
            
            // TODO: Guard class against null objects
            if (_context == null)
            {
                throw new Exception("DataSummit DbContext could not be created");
            }
        }

        public async Task DeleteTemplateAttribute(long templateAttributeId)
        {
            try
            {
                var templateAttribute = new TemplateAttributes() { TemplateAttributeId = templateAttributeId };
                _context.TemplateAttributes.Attach(templateAttribute);
                _context.TemplateAttributes.Remove(templateAttribute);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Documents>> GetAllProjectDocuments(int projectId)
        {
            var documents = new List<Documents>();

            try
            {
                documents = await _context.Documents
                    .Where(d => d.ProjectId == projectId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
            }

            return documents;
        }

        public async Task<List<TemplateAttributes>> GetTemplateAttributesForDocumentId(int documentId)
        {
            var templateAttributes = new List<TemplateAttributes>();

            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
                var templateVersion = await _context.TemplateVersions.FirstOrDefaultAsync(p => p.TemplateVersionId == document.TemplateVersionId);
                templateAttributes = templateVersion.TemplateAttributes.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }

            return templateAttributes;
        }

        public async Task<List<DocumentProperty>> GetDocumentPropertiesByDocumentId(int documentId)
        {
            var documentProperties = new List<DocumentProperty>();
            
            try
            {
                var document = await _context.Documents
                    .FirstOrDefaultAsync(d => d.DocumentId == documentId);

                documentProperties = await _context.Properties
                    .Select(p => new { p.TemplateAttribute, p.Sentence })
                    .Where(profAtrrWords => 
                        profAtrrWords.TemplateAttribute.TemplateVersionId == document.TemplateVersionId 
                        && profAtrrWords.Sentence.DocumentId == document.DocumentId
                    )
                    .Select(a => new DocumentProperty()
                    {
                        Sentences =  a.Sentence,
                        TemplateAttributes = a.TemplateAttribute
                    })
                    .ToListAsync();
            }
            catch (Exception ae)
            {
                throw;
            }

            return documentProperties;
        }

        public async Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue)
        {
            try
            {
                var sentence = new Sentences() { SentenceId = documentPropertyId };
                sentence.Words = documentPropertyValue;
                _context.Sentences.Attach(sentence);
                _context.Entry(sentence).Property("Words").IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Companies
        public async Task<List<Companies>> GetAllCompanies()
        {
            return await _company.GetAllCompanies();
        }

        public async Task CreateCompany(Companies company)
        {
            try
            {
                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateCompany(Companies company)
        {
            try
            {
                _context.Companies.Update(company);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteCompany(int companyId)
        {
            try
            {
                var company = new Companies() { CompanyId = companyId };
                _context.Companies.Attach(company);
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Companies> GetCompanyById(int id)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);
        }
        #endregion

        #region Templates
        public async Task<List<TemplateVersions>> GetCompanyTemplateVersions(int companyId)
        {
            var templateVersions = new List<TemplateVersions>();

            try
            {
                templateVersions = await _context.TemplateVersions
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
                throw;
            }

            return templateVersions;
        }

        public async Task<List<TemplateVersions>> GetProjectTemplateVersions(int projectId)
        {
            var templateVersions = new List<TemplateVersions>();

            try
            {
                templateVersions = await _context.TemplateVersions
                    .Where(pv => pv.Company.Projects
                        .Select(p => p.ProjectId)
                        .Single() == projectId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
                throw;
            }

            return templateVersions;
        }
        #endregion

        #region Projects
        public async Task<List<Projects>> GetAllCompanyProjects(int companyId)
        {
            var projects = new List<Projects>();
            try
            {
                projects = await _context.Projects
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
            }

            return projects;
        }

        public async Task CreateProject(Projects project)
        {
            try
            {
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProjectName(Projects project)
        {
            try
            {
                _context.Projects.Attach(project);
                _context.Entry(project).Property("Name").IsModified = true;
                _context.Entry(project).Property("StorageAccountName").IsModified = false;
                _context.Entry(project).Property("StorageAccountKey").IsModified = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteProject(int projectId)
        {
            try
            {
                var project = new Projects() { ProjectId = projectId };
                _context.Projects.Attach(project);
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Projects> GetProjectById(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        }
        #endregion

        public async Task<List<Documents>> GetProjectDocuments(int projectId)
        {
            var documents = new List<Documents>();

            try
            {
                documents = await _context.Documents
                    .Where(d => d.ProjectId == projectId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
            }

            return documents;
        }

        public async Task<List<TemplateAttributes>> GetTemplateAttribtesForTemplateId(int templateId)
        {
            var templateAttributes = new List<TemplateAttributes>();
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
