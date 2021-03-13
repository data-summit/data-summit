namespace DataSummitModels.DB
{
    public partial class DocumentFeatures
    {
        public DocumentFeatures()
        {
            //Points = new HashSet<Points>();
        }

        public long DocumentFeatureId { get; set; }
        public string Vendor { get; set; }
        public string Value { get; set; }
        public long DocumentId { get; set; }
        public long? Left { get; set; }
        public long? Top { get; set; }
        public long? Width { get; set; }
        public long? Height { get; set; }
        public byte? Feature { get; set; }
        public long? Center { get; set; }

        public virtual Documents Document { get; set; }
        //public virtual ICollection<Points> Points { get; set; }
    }
}
