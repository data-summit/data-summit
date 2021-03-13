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

        public List<Sentences> GetAllDocumentSentences(int documentId, bool IsProdEnvironment = false)
        {
            List<Sentences> Sentences = new List<Sentences>();
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

        public Guid CreateSentence(Sentences sentence, bool IsProdEnvironment = false)
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

        public void UpdateSentence(int id, Sentences sentence, bool IsProdEnvironment = false)
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
                Sentences sentence = dataSummitDbContext.Sentences.First(p => p.SentenceId == SentenceId);
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
