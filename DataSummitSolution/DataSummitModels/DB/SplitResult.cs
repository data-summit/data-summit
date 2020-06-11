using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public class SplitResult
    {
        public List<ImageGrids> grids { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }

        public SplitResult()
        {
            grids = new List<ImageGrids>();
        }
    }
}
