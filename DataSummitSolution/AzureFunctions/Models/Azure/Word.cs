using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;

namespace AzureFunctions.Models.Azure
{
    [Serializable]
    public class Word
    {
        public string BoundingBox { get; set; }
        public string Text { get; set; }
        public Rectangle Rectangle { get; set; }

        public string RectString { get; set; }

        public Word()
        {
            Rectangle = new Rectangle();
        }

        public static Word CastTo(OcrWord word)
        {
            Word w = new Word();
            w.BoundingBox = word.BoundingBox;
            //w.Rectangle = Rectangle.CastTo(word.Rectangle);
            w.RectString = word.BoundingBox;
            w.Text = word.Text;
            return w;
        }
    }
}
