using System.Drawing.Printing;

namespace DataSummitFunctions.Models.Paper
{
    public class Type
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PaperKind Kind { get; set; }

        public Type()
        { }

        public Type(string name)
        { Name = name; }

        public Type(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }

        public Type(string name, int width, int height, PaperKind kind)
        {
            Name = name;
            Width = width;
            Height = height;
            Kind = kind;
        }
    }
}
