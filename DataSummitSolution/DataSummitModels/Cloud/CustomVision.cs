
namespace DataSummitModels.Cloud
{
    public class CustomVision
    {
        public string BlobUrl { get; set; }
        public string MLUrl { get; set; }
        public string TrainingKey { get; set; }
        public string PredictionKey { get; set; }
        public string MLProjectName { get; set; }
        public double MinThreshold { get; set; } = 1;
        public string ContainerName { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageKey { get; set; }


        public CustomVision()
        { ; }
    }
}