using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitModels.DB.Paper
{
    public class Types
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Sizes Size { get; set; }

        public Types()
        { }

        public Types(string name)
        { Name = name; }

        public Types(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }

        public Types(string name, int width, int height, Sizes size)
        {
            Name = name;
            Width = width;
            Height = height;
            Size = size;
        }
    }
}
