using DataSummitService.Classes;
using DataSummitService.Dao.Interfaces;
using DataSummitModels.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitService.Dao
{
    public partial class DataSummitDao : IDataSummitCompaniesDao, IDataSummitProjectsDao, IDataSummitPropertiesDao, IDataSummitTemplateAttributesDao, IDataSummitTemplatesDao
    {
        private readonly DataSummitDbContext _context;

        public DataSummitDao(DataSummitDbContext context)
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


        #region Companies
        public async Task<List<DataSummitModels.DB.Company>> GetAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task CreateCompany(DataSummitModels.DB.Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompany(DataSummitModels.DB.Company company)
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
            return await _context.Companies.SingleOrDefaultAsync(c => c.CompanyId == id);
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

        public async Task CreateProject(DataSummitModels.DB.Project project)
        {
            try
            {
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProjectName(DataSummitModels.DB.Project project)
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
            return await _context.Projects.SingleOrDefaultAsync(p => p.ProjectId == id);
        }
        #endregion

        #region Documents
        public async Task CreateDocument(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public void UpdateDocument(Document document)
        {
            _context.Documents.Update(document);
            _context.SaveChanges();
        }

        public void DeleteDocument(long documentId)
        {
            Document document = _context.Documents.SingleOrDefault(doc => doc.DocumentId == documentId);
            DeleteDocument(document);
        }
        public void DeleteDocument(string documentUrl)
        {
            Document document = _context.Documents.SingleOrDefault(doc => doc.BlobUrl == documentUrl);
            DeleteDocument(document);
        }
        public void DeleteDocument(Document document)
        {
            _context.Documents.Remove(document);
        }

        public async Task DeleteDocumentAsync(long documentId)
        {
            Document document = await _context.Documents.SingleOrDefaultAsync(doc => doc.DocumentId == documentId);
            DeleteDocument(document);
        }
        public async Task DeleteDocumentAsync(string documentUrl)
        {
            Document document = await _context.Documents.SingleOrDefaultAsync(doc => doc.BlobUrl == documentUrl);
            DeleteDocument(document);
        }

        public void AddDocumentFeatures(List<DocumentFeature> documentFeatures)
        {
            documentFeatures.ForEach(doc => AddDocumentFeature(doc));
        }

        public void AddDocumentFeature(DocumentFeature documentFeature)
        {
            _context.DocumentFeatures.Add(documentFeature);
        }

        public async Task UpdateDocumentFeature(string documentUrl, DocumentFeature documentFeature)
        {
            var doc = await _context.Documents.SingleOrDefaultAsync(d => d.BlobUrl == documentUrl);

            doc?.DocumentFeatures.Add(documentFeature);
            _context.SaveChanges();
        }

        public async Task<Document> GetDocumentsByUrlAsync(string documentUrl)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(doc => doc.BlobUrl == documentUrl);
            return document;
        }

        public Document GetDocumentsByUrl(string documentUrl)
        {
            var document = _context.Documents.SingleOrDefault(doc => doc.BlobUrl == documentUrl);
            return document;
        }

        public async Task<List<Document>> GetAllProjectDocuments(int projectId)
        {
            var documents = new List<Document>();

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
                var document = await _context.Documents.SingleOrDefaultAsync(d => d.DocumentId == documentId);
                var templateVersion = await _context.TemplateVersions.SingleOrDefaultAsync(p => p.TemplateVersionId == document.TemplateVersionId);
                templateAttributes = templateVersion.TemplateAttributes.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }

            return templateAttributes;
        }

        public async Task<List<DocumentPropertyDto>> GetDocumentPropertiesByDocumentId(int documentId)
        {
            var documentProperties = new List<DocumentPropertyDto>();
            
            try
            {
                var document = await _context.Documents
                    .SingleOrDefaultAsync(d => d.DocumentId == documentId);

                documentProperties = await _context.Properties
                    .Select(p => new { p.TemplateAttribute, p.Sentence })
                    .Where(profAtrrWords =>
                        profAtrrWords.TemplateAttribute.TemplateVersionId == document.TemplateVersionId
                        && profAtrrWords.Sentence.DocumentId == document.DocumentId
                    )
                    .Select(a => new DocumentPropertyDto()
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

        public async Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue)
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

        //TODO remove this or the 'Task<List<Document>> GetAllProjectDocuments(int projectId)' method. They are duplicates.
        public async Task<List<Document>> GetProjectDocuments(int projectId)
        {
            var documents = new List<Document>();

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
            return await _context.Properties.SingleOrDefaultAsync(p => p.PropertyId == id);
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

        #region Azure URLs
        public async Task<Tuple<string, string>> GetAzureUrlByNameAsync(string name)
        {
            var urlKey = await _context.AzureCompanyResourceUrls.SingleOrDefaultAsync(ar => ar.Name == name);
            return new Tuple<string, string>(urlKey.Url, urlKey.Key);
        }
        public Tuple<string, string> GetAzureUrlByName(string name)
        {
            var urlKey =  _context.AzureCompanyResourceUrls.SingleOrDefault(ar => ar.Name == name);
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
