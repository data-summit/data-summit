namespace DataSummitModels.DB
{
    public partial class DocumentLayers
    {
        public long DocumentLayerId { get; set; }
        public string Name { get; set; }
        public long DocumentId { get; set; }

        public virtual Documents Document { get; set; }

        public DocumentLayers()
        { ; }
    }
}
