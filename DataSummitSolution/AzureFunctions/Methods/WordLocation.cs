using DataSummitModels.DB;
using System;
using System.Collections.Generic;

namespace AzureFunctions.Methods
{
    public static class WordLocation
    {
        public static List<Sentence> Corrected(
                                    List<Sentence> sents, ImageGrid ig)
        {
            List<Sentence> lOut = new List<Sentence>();
            try
            {
                foreach (Sentence s in sents)
                {
                    //s.Rectangle = new Models.Consolidated.Rectangle(
                    //                    ig.WidthStart + s.Rectangle.Left,
                    //                    ig.HeightStart + s.Rectangle.Top,
                    //                    s.Rectangle.Width, s.Rectangle.Height);
                    s.Left = ig.WidthStart + s.Left;
                    s.Top = ig.HeightStart + s.Top;
                    s.Width = s.Width;
                    s.Height = s.Height;
                    s.IsUsed = true;

                    //TODO correct
                    //lOut.Add(s.ToModel());
                    { }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lOut;
        }

        internal static IEnumerable<Sentence> Corrected(List<DataSummitModels.Cloud.Consolidated.Sentences> sentences, ImageGrid ig)
        {
            throw new NotImplementedException();
        }
    }
}
