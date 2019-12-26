using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;

namespace TemplateMapper.OCR.Azure
{
    [Serializable]
    public class Line
    {
        public string BoundingBox { get; set; }
        public List<Word> Words = new List<Word>();
        public Rectangle Rectangle { get; set; }
        public string RectString { get; set; }

        public Line()
        {
            Rectangle = new Rectangle();
        }
        public Line(string boundingbox, string word)
        {
            Rectangle = new Rectangle();
            BoundingBox = boundingbox;
            Word w = new Word();
            w.Text = word;
            Words.Add(w);
        }

        public static Line CastTo(OcrLine line)
        {
            Line l = new Line();
            l.BoundingBox = line.BoundingBox;
            //l.Rectangle = Rectangle.CastTo(line.BoundingBox);
            l.RectString = line.BoundingBox;
            foreach (OcrWord w in line.Words)
            { l.Words.Add(Word.CastTo(w)); }
            return l;
        }
    }
}
