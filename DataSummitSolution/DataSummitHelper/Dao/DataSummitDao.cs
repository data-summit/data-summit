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

        public async Task DeleteProfileAttribute(long profileAttributeId)
        {
            try
            {
                var profileAttribute = new ProfileAttributes() { ProfileAttributeId = profileAttributeId };
                _context.ProfileAttributes.Attach(profileAttribute);
                _context.ProfileAttributes.Remove(profileAttribute);
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

        public async Task<List<ProfileAttributes>> GetProfileAttributesForDocumentId(int documentId)
        {
            var profileAttributes = new List<ProfileAttributes>();

            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
                var profileVersion = await _context.ProfileVersions.FirstOrDefaultAsync(p => p.ProfileVersionId == document.ProfileVersionId);
                profileAttributes = profileVersion.ProfileAttributes.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }

            return profileAttributes;
        }

        public async Task<List<DocumentProperty>> GetDocumentPropertiesByDocumentId(int documentId)
        {
            var documentProperties = new List<DocumentProperty>();
            
            try
            {
                var document = await _context.Documents
                    .FirstOrDefaultAsync(d => d.DocumentId == documentId);

                documentProperties = await _context.Properties
                    .Select(p => new { p.ProfileAttribute, p.Sentence })
                    .Where(profAtrrWords => 
                        profAtrrWords.ProfileAttribute.ProfileVersionId == document.ProfileVersionId 
                        && profAtrrWords.Sentence.DocumentId == document.DocumentId
                    )
                    .Select(a => new DocumentProperty()
                    {
                        Sentences =  a.Sentence,
                        ProfileAttributes = a.ProfileAttribute
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
        public async Task<List<ProfileVersions>> GetCompanyProfileVersions(int companyId)
        {
            var profileVersions = new List<ProfileVersions>();

            try
            {
                profileVersions = await _context.ProfileVersions
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
                throw;
            }

            return profileVersions;
        }

        public async Task<List<ProfileVersions>> GetProjectProfileVersions(int projectId)
        {
            var profileVersions = new List<ProfileVersions>();

            try
            {
                profileVersions = await _context.ProfileVersions
                    .Where(pv => pv.Company.Projects
                        .Select(p => p.ProjectId)
                        .Single() == projectId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
                throw;
            }

            return profileVersions;
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

        public async Task<List<ProfileAttributes>> GetTemplateAttribtesForTemplateId(int templateId)
        {
            var profileAttributes = new List<ProfileAttributes>();
            try
            {
                profileAttributes = await _context.ProfileAttributes
                    .Where(p => p.ProfileVersionId == templateId)
                    .ToListAsync();
            }
            catch (Exception ae)
            {
            }

            return profileAttributes;
        }
    }
}
