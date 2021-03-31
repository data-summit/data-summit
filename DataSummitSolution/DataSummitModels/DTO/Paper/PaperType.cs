using DataSummitModels.Enums;

namespace DataSummitModels.DTO.Paper
{
    public class PaperType
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PaperSize Size { get; set; }

        public PaperType()
        { }

        public PaperType(string name)
        { Name = name; }

        public PaperType(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }

        public PaperType(string name, int width, int height, PaperSize size)
        {
            Name = name;
            Width = width;
            Height = height;
            Size = size;
        }
    }
}
