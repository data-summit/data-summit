using System;
using System.IO;

namespace TemplateMapper
{
    public class Tile
    {
        public String FileName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public AreaType Type { get; set; }
        public MemoryStream Stream { get; set; }
        public byte[] ImageData { get; set; }

        public Tile()
        {
            Stream = new MemoryStream();
        }

        public Tile(String filename, int x, int y, int width, int height, AreaType areatype, MemoryStream stream)
        {
            FileName = filename;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = areatype;
            Stream = stream;
        }
        public enum AreaType
        {
            Normal,
            Overlap
        }
    }
}
