using DataSummitModels.DB;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitModels.DTO
{
    public partial class Sentences : Sentence
    {
        public Sentence FromCloudToDB(Cloud.Consolidated.Sentences sentence)
        {
            Sentence s = new Sentence()
            {
                Confidence = sentence.Confidence,
                Document = sentence.Document,
                DocumentId = sentence.DocumentId,
                Height = sentence.Height,
                IsUsed = sentence.IsUsed,
                Left = sentence.Left,
                //ModifiedWords = sentence.ModifiedWords,
                Properties = sentence.Properties.Select(sen => sen.ToModel()).ToList(),
                SentenceId = sentence.SentenceId,
                SlendernessRatio = sentence.SlendernessRatio,
                Top = sentence.Top,
                Vendor = sentence.Vendor,
                Width = sentence.Width,
                Words = sentence.Words
            };
            return s;
        }

        public List<Sentence> FromCloudToDB(List<Cloud.Consolidated.Sentences> sentences)
        {
            List<Sentence> ls = new List<Sentence>();
            foreach (var sentence in sentences)
            {
                ls.Add(FromCloudToDB(sentence));
            }
            return ls;
        }
    }
}
