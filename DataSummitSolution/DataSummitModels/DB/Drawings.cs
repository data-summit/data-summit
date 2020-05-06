using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class Drawings : DrawingBase
    {
        public Drawings()
        {
            
            DrawingFeatures = new HashSet<DrawingFeatures>();
            ImageGrids = new HashSet<ImageGrids>();
        }

        public virtual PaperOrientations PaperOrientation { get; set; }
        public virtual PaperSizes PaperSize { get; set; }
        public virtual Projects Project { get; set; }
        
        public virtual ICollection<DrawingFeatures> DrawingFeatures { get; set; }
        public virtual ICollection<ImageGrids> ImageGrids { get; set; }


        public ImageUpload ToImageUpload()
        {
            ImageUpload imgU = new ImageUpload();
            imgU.AmazonConfidence = AmazonConfidence;
            imgU.BlobUrl = BlobUrl;
            imgU.ContainerName = ContainerName;
            imgU.CreatedDate = CreatedDate;
            imgU.DrawingId = DrawingId;
            imgU.File = File;
            imgU.FileName = FileName;
            imgU.GoogleConfidence = GoogleConfidence;
            imgU.AzureConfidence = AzureConfidence;
            imgU.Processed = Processed;
            imgU.ProfileVersionId = ProfileVersionId;
            imgU.ProjectId = ProjectId;
            imgU.Sentences = Sentences;
            imgU.Success = Success;
            imgU.Type = Type;
            imgU.Tasks = Tasks;
            imgU.UserId = UserId;
            return imgU;
        }
    }
}
