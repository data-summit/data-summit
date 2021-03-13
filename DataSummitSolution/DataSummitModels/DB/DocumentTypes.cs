namespace DataSummitModels.DB
{
    public partial class DocumentTypes
    {
        public int DocumentTypeId { get; set; }
        public string Name { get; set; }

        public virtual Documents Document { get; set; }

        public DocumentTypes()
        { ; }
    }
}
