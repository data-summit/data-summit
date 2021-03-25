
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

        public enum Extension
        {
            Unknown = 1,
            JPG = 2,
            PNG = 3,
            PDF = 4,
            GIF = 5
        }
    }
}
