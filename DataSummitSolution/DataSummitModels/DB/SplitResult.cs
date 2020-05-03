using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public class SplitResult
    {
        public List<ImageGrids> grids { get; set; }
        public string BlobStorageName { get; set; }
        public string BlobStorageKey { get; set; }

        public SplitResult()
        {
            grids = new List<ImageGrids>();
        }
    }
}
