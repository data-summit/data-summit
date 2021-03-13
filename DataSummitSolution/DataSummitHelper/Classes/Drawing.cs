using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class Document
    {
        DataSummitDbContext dataSummitDbContext;

        public Document(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public long DocumentCount()
        {
            return dataSummitDbContext.Documents.Count();
        }

        public List<Documents> GetAllCompanyDocuments(int projectId)
        {
            var documents = new List<Documents>();
            try
            {
                documents = dataSummitDbContext.Documents
                    .Where(e => e.ProjectId == projectId)
                    .ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return documents;
        }

        public long CreateDocument(List<Documents> documents)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                foreach (Documents document in documents)
                {
                    dataSummitDbContext.Documents.Add(document);
                    dataSummitDbContext.SaveChanges();
                    returnid = document.DocumentId;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return returnid;
        }

        public void UpdateDocument(long id, Documents document)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Documents.Update(document);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
        public void DeleteDocument(long documentId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                Documents document = dataSummitDbContext.Documents.First(p => p.DocumentId == documentId);
                dataSummitDbContext.Documents.Remove(document);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}