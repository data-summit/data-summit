
namespace DataSummitModels.DB
{
    public partial class AzureMLResource
    {
        public long AzureMLResourcesId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string TrainingKey { get; set; }
        public string PredicitionKey { get; set; }
    }
}
