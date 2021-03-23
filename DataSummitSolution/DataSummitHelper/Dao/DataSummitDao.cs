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
        private readonly Classes.Company _company;

        public DataSummitDao(DataSummitDbContext context)
        {
            _context = context;
            _company = new Classes.Company(_context);
            
            // TODO: Guard class against null objects
            if (_context == null)
            {
                throw new Exception("DataSummit DbContext could not be created");
            }
        }

        public async System.Threading.Tasks.Task DeleteTemplateAttribute(long templateAttributeId)
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


        #region Companies
        public async Task<List<DataSummitModels.DB.Company>> GetAllCompanies()
        {
            return await _company.GetAllCompanies();
        }

        public async System.Threading.Tasks.Task CreateCompany(DataSummitModels.DB.Company company)
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

        public async System.Threading.Tasks.Task UpdateCompany(DataSummitModels.DB.Company company)
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

        public async System.Threading.Tasks.Task DeleteCompany(int companyId)
        {
            try
            {
                var company = new DataSummitModels.DB.Company() { CompanyId = companyId };
                _context.Companies.Attach(company);
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<DataSummitModels.DB.Company> GetCompanyById(int id)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);
        }
        #endregion

        #region Templates
        public async Task<List<DataSummitModels.DB.TemplateVersion>> GetCompanyTemplateVersions(int companyId)
        {
            var templateVersions = new List<DataSummitModels.DB.TemplateVersion>();

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

        public async Task<List<DataSummitModels.DB.TemplateVersion>> GetProjectTemplateVersions(int projectId)
        {
            var templateVersions = new List<DataSummitModels.DB.TemplateVersion>();

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
        public async Task<List<DataSummitModels.DB.Project>> GetAllCompanyProjects(int companyId)
        {
            var projects = new List<DataSummitModels.DB.Project>();
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

        public async System.Threading.Tasks.Task CreateProject(DataSummitModels.DB.Project project)
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

        public async System.Threading.Tasks.Task UpdateProjectName(DataSummitModels.DB.Project project)
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

        public async System.Threading.Tasks.Task DeleteProject(int projectId)
        {
            try
            {
                var project = new DataSummitModels.DB.Project() { ProjectId = projectId };
                _context.Projects.Attach(project);
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataSummitModels.DB.Project> GetProjectById(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        }
        #endregion

        #region Documents
        public async Task<List<DataSummitModels.DB.Document>> GetAllProjectDocuments(int projectId)
        {
            var documents = new List<DataSummitModels.DB.Document>();

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

        public async Task<List<DataSummitModels.DB.TemplateAttribute>> GetAttributesForDocumentId(int documentId)
        {
            var templateAttributes = new List<DataSummitModels.DB.TemplateAttribute>();

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
                        Sentences = new DataSummitModels.Cloud.Consolidated.Sentences()
                        {
                            Confidence = a.Sentence.Confidence,
                            DocumentId = a.Sentence.DocumentId,
                            Height = a.Sentence.Height,
                            IsUsed = a.Sentence.IsUsed,
                            Left = a.Sentence.Left,
                            //Properties = a.Sentence.Properties,
                            SentenceId = a.Sentence.SentenceId,
                            SlendernessRatio = (decimal)a.Sentence.SlendernessRatio,
                            Top = a.Sentence.Top,
                            Vendor = a.Sentence.Vendor,
                            Width = a.Sentence.Width,
                            Words = a.Sentence.Words

                        },
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

        public async System.Threading.Tasks.Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue)
        {
            try
            {
                var sentence = new DataSummitModels.DB.Sentence() { SentenceId = documentPropertyId };
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

        public async Task<List<DataSummitModels.DB.Document>> GetProjectDocuments(int projectId)
        {
            var documents = new List<DataSummitModels.DB.Document>();

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
        #endregion

        #region Properties
        public async Task<DataSummitModels.DB.Property> GetPropertyById(int id)
        {
            return await _context.Properties.FirstOrDefaultAsync(p => p.PropertyId == id);
        }
        public async Task<bool> DeleteProperty(long propertyId)
        {
            bool result;
            try
            {
                var property = new DataSummitModels.DB.Property() { PropertyId = propertyId };
                _context.Properties.Attach(property);
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch
            {
                throw;
            }
            return result;
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
