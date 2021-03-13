using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class Sentence
    {
        DataSummitDbContext dataSummitDbContext;

        public Sentence(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.Sentence> GetAllDocumentSentences(int documentId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.DB.Sentence> Sentences = new List<DataSummitModels.DB.Sentence>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                Sentences = dataSummitDbContext.Sentences
                                .Where(e => e.DocumentId == documentId)
                                .ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Sentences;
        }

        public Guid CreateSentence(DataSummitModels.DB.Sentence sentence, bool IsProdEnvironment = false)
        {
            Guid returnGuid = Guid.NewGuid();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Sentences.Add(sentence);
                dataSummitDbContext.SaveChanges();
                returnGuid = sentence.SentenceId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnGuid;
        }

        public void UpdateSentence(int id, DataSummitModels.DB.Sentence sentence, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Sentences.Update(sentence);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteSentence(Guid SentenceId, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.Sentence sentence = dataSummitDbContext.Sentences.First(p => p.SentenceId == SentenceId);
                dataSummitDbContext.Sentences.Remove(sentence);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
