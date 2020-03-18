using DataSummitModels.DB;
using DataSummitModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Properties
    {
        DataSummitDbContext dataSummitDbContext;

        public Properties(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public DrawingData GetAllDrawingProperties(int drawingId)
        {
            DrawingData dp = new DrawingData();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();

                DataSummitModels.DB.Drawings Drawing = dataSummitDbContext.
                    Drawings.FirstOrDefault(d => d.DrawingId == drawingId);
                DataSummitModels.DB.ProfileVersions Template = dataSummitDbContext.
                    ProfileVersions.FirstOrDefault(t => t.ProfileVersionId == Drawing.ProfileVersionId);

                List<DataSummitModels.DB.ProfileAttributes> Attributes = dataSummitDbContext.
                    ProfileAttributes.Where(a => a.ProfileVersionId == Template.ProfileVersionId).ToList();
                List<DataSummitModels.DB.Sentences> sentences = dataSummitDbContext.
                    Sentences.Where(s => s.DrawingId == drawingId).ToList();
                List<DataSummitModels.DB.Properties> Properties = dataSummitDbContext
                    .Properties.Where(p => sentences.Select(s => s.SentenceId).Contains(p.SentenceId) == true).ToList();
                List<DataSummitModels.DB.StandardAttributes> standardAttributes = dataSummitDbContext
                    .StandardAttributes.ToList();

                //Map Drawing properties
                dp.AmazonConfidence = Drawing.AmazonConfidence;
                dp.AzureConfidence = Drawing.AzureConfidence;
                dp.BlobUrl = Drawing.BlobUrl;
                dp.ContainerName = Drawing.ContainerName;
                dp.CreatedDate = Drawing.CreatedDate;
                dp.DrawingId = Drawing.DrawingId;
                dp.FileName = Drawing.FileName;
                dp.GoogleConfidence = Drawing.GoogleConfidence;
                dp.PaperOrientationId = Drawing.PaperOrientationId;
                dp.PaperSizeId = Drawing.PaperSizeId;
                dp.Processed = Drawing.Processed;
                dp.ProfileVersionId = Drawing.ProfileVersionId;
                dp.ProjectId = Drawing.ProjectId;
                dp.Success = Drawing.Success;
                dp.Type = Drawing.Type;
                dp.UserId = Drawing.UserId;

                //Map ProfileVersions properties
                dp.Name = Template.Name;
                dp.CompanyId = Template.CompanyId;
                dp.Width = Template.Width;
                dp.Height = Template.Height;
                dp.WidthOriginal = Template.WidthOriginal;
                dp.HeightOriginal = Template.HeightOriginal;
                dp.X = Template.X;
                dp.Y = Template.Y;

                if (Properties.Count > 0 && dp.Properties == null) dp.Properties = new List<PropertyData>();
                foreach(var p in Properties)
                {
                    DataSummitModels.DB.Sentences sentence = sentences.FirstOrDefault(s => s.SentenceId == p.SentenceId);
                    DataSummitModels.DB.ProfileAttributes attribute = Attributes.FirstOrDefault(a => a.ProfileAttributeId == p.ProfileAttributeId);
                    if (sentence != null && attribute != null)
                    {
                        PropertyData pd = new PropertyData();
                        DataSummitModels.DB.StandardAttributes standard = standardAttributes.FirstOrDefault(sa => sa.StandardAttributeId == attribute.StandardAttributeId);

                        //Map Sentence class properties
                        pd.SentenceId = sentence.SentenceId;
                        pd.Words = sentence.Words;
                        pd.Width = sentence.Width;
                        pd.Height = sentence.Height;
                        pd.Left = sentence.Left;
                        pd.Top = sentence.Top;
                        pd.Vendor = sentence.Vendor;
                        pd.IsUsed = sentence.IsUsed;
                        pd.Confidence = sentence.Confidence;
                        pd.SlendernessRatio = sentence.SlendernessRatio;
                        pd.DrawingId = sentence.DrawingId;

                        //Map ProfileAttribute class properties
                        pd.ProfileAttributeId = attribute.ProfileAttributeId;
                        pd.Name = attribute.Name;
                        pd.NameX = attribute.NameX;
                        pd.NameY = attribute.NameY;
                        pd.NameWidth = attribute.NameWidth;
                        pd.NameHeight = attribute.NameHeight;
                        pd.PaperSizeId = attribute.PaperSizeId;
                        pd.BlockPositionId = attribute.BlockPositionId;
                        pd.ProfileVersionId = attribute.ProfileVersionId;
                        pd.CreatedDate = attribute.CreatedDate;
                        pd.UserId = attribute.UserId;
                        pd.Value = attribute.Value;
                        pd.ValueX = attribute.ValueX;
                        pd.ValueY = attribute.ValueY;
                        pd.ValueWidth = attribute.ValueWidth;
                        pd.ValueHeight = attribute.ValueHeight;

                        //Map StandardAttribute properties
                        pd.StandardAttributeId = standard.StandardAttributeId;
                        pd.StandardName = standard.Name;

                        dp.Properties.Add(pd);
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return dp;
        }

        public long CreateProperty(DataSummitModels.DB.Properties property)
        {
            long returnlong = long.MinValue;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                dataSummitDbContext.Properties.Add(property);
                dataSummitDbContext.SaveChanges();
                returnlong = property.PropertyId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnlong;
        }

        public void UpdateProperty(int id, DataSummitModels.DB.Properties property)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                dataSummitDbContext.Properties.Update(property);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteProperty(long propertyId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitModels.DB.DataSummitDbContext();
                DataSummitModels.DB.Properties property = dataSummitDbContext.Properties.First(
                                                            p => p.PropertyId == propertyId);
                dataSummitDbContext.Properties.Remove(property);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
