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

        public List<DataSummitModels.DB.Document> GetAllCompanyDocuments(int projectId)
        {
            var documents = new List<DataSummitModels.DB.Document>();
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

        public long CreateDocument(List<DataSummitModels.DB.Document> documents)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                foreach (DataSummitModels.DB.Document document in documents)
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
    }
}