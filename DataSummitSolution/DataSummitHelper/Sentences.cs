using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Sentences
    {
        DataSummitDbContext dataSummitDbContext;

        public Sentences(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.Sentences> GetAllDrawingSentences(int drawingId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.DB.Sentences> Sentences = new List<DataSummitModels.DB.Sentences>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                Sentences = dataSummitDbContext.Sentences
                                .Where(e => e.DrawingId == drawingId)
                                .ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Sentences;
        }

        public Guid CreateSentence(DataSummitModels.DB.Sentences sentence, bool IsProdEnvironment = false)
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

        public void UpdateSentence(int id, DataSummitModels.DB.Sentences sentence, bool IsProdEnvironment = false)
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
                DataSummitModels.DB.Sentences sentence = dataSummitDbContext.Sentences.First(p => p.SentenceId == SentenceId);
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
