using DataSummitModels.Cloud.Consolidated;
using DataSummitModels.DB;

namespace DataSummitService.Classes
{
    /// <summary>
    /// </summary>
    public sealed class DocumentPropertyDto
    {
        public Sentences Sentences { get; set; }
        public TemplateAttribute TemplateAttributes { get; set; }
    }
}
