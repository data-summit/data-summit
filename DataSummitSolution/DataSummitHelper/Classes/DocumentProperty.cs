using DataSummitModels.Cloud.Consolidated;

namespace DataSummitHelper.Classes
{
    /// <summary>
    /// </summary>
    public sealed class DocumentProperty
    {
        public Sentences Sentences { get; set; }
        public DataSummitModels.DB.TemplateAttribute TemplateAttributes { get; set; }
    }
}
