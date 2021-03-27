
namespace DataSummitModels.DB
{
    public partial class DocumentFeature
    {
        public long DocumentFeatureId { get; set; }
        public string Vendor { get; set; }
        public string Value { get; set; }
        public long DocumentId { get; set; }
        public long? Left { get; set; }
        public long? Top { get; set; }
        public long? Width { get; set; }
        public long? Height { get; set; }
        public string Feature { get; set; }
        public double Confidence { get; set; } = 0;
        public long? Center { get; set; }

        public virtual Document Document { get; set; }
    }
}