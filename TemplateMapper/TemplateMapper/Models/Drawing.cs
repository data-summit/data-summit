using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    public partial class Drawing : DrawingBase
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Drawing()
        {
            DrawingLayer = new HashSet<DrawingLayer>();
            //DrawingVTemplate = new HashSet<DrawingVTemplate>();
            ImageGrid = new HashSet<ImageGrid>();
            //PaperSize = new HashSet<PaperSize>();
            TaskDuration = new HashSet<TaskDuration>();
        }

        //public virtual Project Project { get; set; }
        public virtual ICollection<DrawingLayer> DrawingLayer { get; set; }
        //public virtual ICollection<DrawingVTemplate> DrawingVTemplate { get; set; }
        public virtual ICollection<ImageGrid> ImageGrid { get; set; }
        //public virtual ICollection<PaperSize> PaperSize { get; set; }
        public virtual ICollection<TaskDuration> TaskDuration { get; set; }

        public ImageUpload ToImageUpload()
        {
            ImageUpload imgU = new ImageUpload
            {
                AmazonConfidence = AmazonConfidence,
                BlobURL = BlobURL,
                ContainerName = ContainerName,
                CreatedDate = CreatedDate,
                DrawingId = DrawingId,
                File = File,
                FileName = FileName,
                GoogleConfidence = GoogleConfidence,
                AzureConfidence = AzureConfidence,
                Processed = Processed,
                ProfileVersionId = ProfileVersionId,
                ProjectId = ProjectId,
                Success = Success,
                Type = Type,
                Tasks = Tasks,
                UserId = UserId
            };
            return imgU;
        }
    }
}
