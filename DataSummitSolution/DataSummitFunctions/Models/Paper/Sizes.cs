using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitFunctions.Models.Paper
{
    public static class Sizes
    {
        public static List<Type> All = new List<Type>();

        public static void Load()
        {
            All.Add(new Type("A0", 841, 1189, PaperKind.A0));
            All.Add(new Type("A1", 594, 841, PaperKind.A1));
            All.Add(new Type("A2", 420, 594, PaperKind.A2));
            All.Add(new Type("A3", 297, 420, PaperKind.A3));
            All.Add(new Type("A3 extra", 322, 445, PaperKind.A3Extra));
            All.Add(new Type("A3 extra transverse", 322, 445, PaperKind.A3ExtraTransverse));
            All.Add(new Type("A3 rotated", 420, 297, PaperKind.A3Rotated));
            All.Add(new Type("A3 transverse", 297, 420, PaperKind.A3Transverse));
            All.Add(new Type("A4", 210, 297, PaperKind.A4));
            All.Add(new Type("A4 extra", 236, 322, PaperKind.A4Extra));
            All.Add(new Type("A4 plus", 210, 330, PaperKind.A4Plus));
            All.Add(new Type("A4 rotated", 297, 210, PaperKind.A4Rotated));
            All.Add(new Type("A4 small", 210, 297, PaperKind.A4Small));
            All.Add(new Type("A4 transverse", 210, 297, PaperKind.A4Transverse));
            All.Add(new Type("A5", 148, 210, PaperKind.A5));
            All.Add(new Type("A5 extra", 174, 235, PaperKind.A5Extra));
            All.Add(new Type("A5 rotated", 210, 148, PaperKind.A5Rotated));
            All.Add(new Type("A5 transverse", 148, 210, PaperKind.A5Transverse));
            All.Add(new Type("A6", 105, 148, PaperKind.A6));
            All.Add(new Type("A6 rotated", 148, 105, PaperKind.A6Rotated));
            All.Add(new Type("SuperA/SuperA/A4", 227, 356, PaperKind.APlus));
            All.Add(new Type("B4", 250, 353, PaperKind.B4));
            All.Add(new Type("B4 envelope", 250, 353, PaperKind.B4Envelope));
            All.Add(new Type("JIS B4 rotated", 364, 257, PaperKind.B4JisRotated));
            All.Add(new Type("B5", 176, 250, PaperKind.B5));
            All.Add(new Type("B5 envelope", 176, 250, PaperKind.B5Envelope));
            All.Add(new Type("ISO B5 extra", 201, 276, PaperKind.B5Extra));
            All.Add(new Type("JIS B5 rotated", 257, 182, PaperKind.B5JisRotated));
            All.Add(new Type("JIS B5 transverse", 182, 257, PaperKind.B5Transverse));
            All.Add(new Type("B6 envelope", 176, 125, PaperKind.B6Envelope));
            All.Add(new Type("JIS B6", 128, 182, PaperKind.B6Jis));
            All.Add(new Type("JIS B6 rotated", 182, 128, PaperKind.B6JisRotated));
            All.Add(new Type("SuperB/SuperB/A3", 305, 487, PaperKind.BPlus));
            All.Add(new Type("C3 envelope", 324, 458, PaperKind.C3Envelope));
            All.Add(new Type("C4 envelope", 229, 324, PaperKind.C4Envelope));
            All.Add(new Type("C5 envelope", 162, 229, PaperKind.C5Envelope));
            All.Add(new Type("C65 envelope", 114, 229, PaperKind.C65Envelope));
            All.Add(new Type("C6 envelope", 114, 162, PaperKind.C6Envelope));
            All.Add(new Type("C", 432, 559, PaperKind.CSheet));
            All.Add(new Type("DL envelope", 110, 220, PaperKind.DLEnvelope));
            All.Add(new Type("D", 559, 864, PaperKind.DSheet));
            All.Add(new Type("E", 864, 1118, PaperKind.ESheet));
            All.Add(new Type("Executive", 184, 267, PaperKind.Executive));
            All.Add(new Type("Folio", 216, 330, PaperKind.Folio));
            All.Add(new Type("German legal fanfold", 216, 330, PaperKind.GermanLegalFanfold));
            All.Add(new Type("German standard fanfold", 216, 305, PaperKind.GermanStandardFanfold));
            All.Add(new Type("Invitation envelope", 220, 220, PaperKind.InviteEnvelope));
            All.Add(new Type("ISO B4", 250, 353, PaperKind.IsoB4));
            All.Add(new Type("Italy envelope", 110, 230, PaperKind.ItalyEnvelope));
            All.Add(new Type("Ledger", 432, 279, PaperKind.Ledger));
            All.Add(new Type("Legal", 216, 356, PaperKind.Legal));
            All.Add(new Type("Letter", 216, 279, PaperKind.Letter));
            All.Add(new Type("Letter extra transverse", 236, 305, PaperKind.LetterExtraTransverse));
            All.Add(new Type("Letter plus", 216, 322, PaperKind.LetterPlus));
            All.Add(new Type("Letter rotated", 279, 216, PaperKind.LetterRotated));
            All.Add(new Type("Letter small", 216, 279, PaperKind.LetterSmall));
            All.Add(new Type("Letter transverse", 210, 279, PaperKind.LetterTransverse));
            All.Add(new Type("Monarch envelope", 98, 191, PaperKind.MonarchEnvelope));
            All.Add(new Type("Note", 216, 279, PaperKind.Note));
            All.Add(new Type("#10 envelope", 105, 241, PaperKind.Number10Envelope));
            All.Add(new Type("#11 envelope", 114, 264, PaperKind.Number11Envelope));
            All.Add(new Type("#12 envelope", 121, 279, PaperKind.Number12Envelope));
            All.Add(new Type("#14 envelope", 127, 292, PaperKind.Number14Envelope));
            All.Add(new Type("#9 envelope", 98, 225, PaperKind.Number9Envelope));
            All.Add(new Type("6 3/4 envelope", 92, 165, PaperKind.PersonalEnvelope));
            All.Add(new Type("Quarto", 215, 275, PaperKind.Quarto));
            All.Add(new Type("Standard", 254, 279, PaperKind.Standard10x11));
            All.Add(new Type("Standard", 254, 356, PaperKind.Standard10x14));
            All.Add(new Type("Standard", 279, 432, PaperKind.Standard11x17));
            All.Add(new Type("Standard", 305, 279, PaperKind.Standard12x11));
            All.Add(new Type("Standard", 381, 279, PaperKind.Standard15x11));
            All.Add(new Type("Standard", 229, 279, PaperKind.Standard9x11));
            All.Add(new Type("Statement", 140, 216, PaperKind.Statement));
            All.Add(new Type("Tabloid", 279, 432, PaperKind.Tabloid));
            All.Add(new Type("Tabloid extra", 297, 457, PaperKind.TabloidExtra));
            All.Add(new Type("US standard fanfold", 378, 279, PaperKind.USStandardFanfold));
        }

        public static PaperKind Match(float widthPixel, float heightPixel, short tolerance = 5)
        {
            List<Type> lPossibleMatches = new List<Type>();
            try
            {
                int width = (int)Math.Round(widthPixel * 0.352777777777, 0);
                int height = (int)Math.Round(heightPixel * 0.352777777777, 0);

                lPossibleMatches.AddRange(
                    All.Where(p => 
                    (p.Height - tolerance) <= height && (p.Height + tolerance) >= height &&
                    (p.Width - tolerance) <= width && (p.Width + tolerance) >= width)
                    .ToList());

                if (lPossibleMatches.Count > 1)
                {
                    Type Match = ReduceMultiMatch(lPossibleMatches, width, height, tolerance);
                    return Match.Kind;
                }
                else if (lPossibleMatches.Count == 1)
                {
                    return lPossibleMatches[0].Kind;
                }

                //Find matches for landscape orientation
                if (lPossibleMatches.Count == 0)
                {
                    lPossibleMatches.AddRange(
                        All.Where(p =>
                        (p.Height - tolerance) <= width && (p.Height + tolerance) >= width &&
                        (p.Width - tolerance) <= height && (p.Width + tolerance) >= height)
                        .ToList());

                    if (lPossibleMatches.Count > 1)
                    {
                        Type Match = ReduceMultiMatch(lPossibleMatches, height, width, tolerance);
                    }
                    else if (lPossibleMatches.Count == 1)
                    {
                        return lPossibleMatches[0].Kind;
                    }
                }
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return PaperKind.Custom;
        }

        private static Type ReduceMultiMatch(List<Type> lMatches, int width, int height, short tolerance)
        {
            try
            {
                for (short i = 0; i < tolerance; i++)
                {
                    if (All.Count(p =>
                        (p.Height - i) <= width && (p.Height + i) >= width &&
                        (p.Width - i) <= height && (p.Width + i) >= height) == 1)
                    {
                        return All.First(p =>
                            (p.Height - i) <= width && (p.Height + i) >= width &&
                            (p.Width - i) <= height && (p.Width + i) >= height);
                    }
                    else if (All.Count(p =>
                        (p.Height - i) <= width && (p.Height + i) >= width &&
                        (p.Width - i) <= height && (p.Width + i) >= height) == 1)
                    {
                        return All.First(p =>
                            (p.Height - i) <= width && (p.Height + i) >= width &&
                            (p.Width - i) <= height && (p.Width + i) >= height);
                    }
                }
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return lMatches[0];
        }
    }
}
