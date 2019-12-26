using DataSummitFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Methods
{
    public static class WordLocation
    {
        public static List<Models.Consolidated.Sentences> Corrected(
                                    List<Models.Consolidated.Sentences> sents, ImageGrid ig)
        {
            List<Models.Consolidated.Sentences> lOut = new List<Models.Consolidated.Sentences>();
            try
            {
                foreach (Models.Consolidated.Sentences s in sents)
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
                    lOut.Add(s);

                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lOut;
        }
    }
}
