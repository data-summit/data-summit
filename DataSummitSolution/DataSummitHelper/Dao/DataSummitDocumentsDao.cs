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
    public partial class DataSummitDocumentsDao : IDataSummitDocumentsDao
    {
        private readonly DataSummitDbContext _context;

        public DataSummitDocumentsDao(DataSummitDbContext context)
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
            var doc = await _context.Documents
                                    .Include(d => d.DocumentFeatures)
                                    .SingleOrDefaultAsync(d => d.BlobUrl == documentUrl);

            doc?.DocumentFeatures.Add(documentFeature);
            _context.SaveChanges();
        }

        public async Task UpdateDocumentFeatures(string documentUrl, List<DocumentFeature> features)
        {
            var doc = await _context.Documents
                                    .Include(d => d.DocumentFeatures)
                                    .SingleOrDefaultAsync(d => d.BlobUrl == documentUrl);

            if (doc == null)
            { return; }

            features.ForEach(f => f.DocumentId = doc.DocumentId);
            doc.DocumentFeatures = doc.DocumentFeatures
                                      .Union(features)
                                      .ToList();
            _context.SaveChanges();
        }

        public async Task<Document> GetDocumentsByUrlAsync(string documentUrl)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(doc => doc.BlobUrl == documentUrl);
            return document;
        }

        public Document GetDocumentByUrl(string documentUrl)
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

        public async Task<List<TemplateAttribute>> GetAttributesForDocumentId(int documentId)
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
    }
}
