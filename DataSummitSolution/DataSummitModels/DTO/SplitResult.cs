using DataSummitModels.DB;
using System.Collections.Generic;

namespace DataSummitModels.DTO
{
    public class SplitResult
    {
        public List<ImageGrid> grids { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }

        public SplitResult()
        {
            grids = new List<ImageGrid>();
        }
    }
}
