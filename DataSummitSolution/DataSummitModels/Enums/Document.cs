
namespace DataSummitModels.Enums
{
    public class Document
    {
        public enum Type
        {
            Unknown = 1,
            DrawingPlanView = 2,
            Gantt = 3,
            Report = 4,
            Schematic = 5
        }

        public enum Format
        {
            Unknown = 4,
            JPG = 1,
            PNG = 2,
            PDF = 3
        }
    }
}
