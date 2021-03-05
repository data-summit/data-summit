using System;
using System.Collections.Generic;
using System.Linq;
using static DataSummitModels.Enums.Paper;

namespace DataSummitModels.DB.Paper
{
    public static class Sizes
    {
        public static List<Types> All = new List<Types>();

        public static void Load()
        {
            All.Add(new Types("A0", 841, 1189, Size.A0));
            All.Add(new Types("A1", 594, 841, Size.A1));
            All.Add(new Types("A2", 420, 594, Size.A2));
            All.Add(new Types("A3", 297, 420, Size.A3));
            All.Add(new Types("A3 extra", 322, 445, Size.A3Extra));
            All.Add(new Types("A3 extra transverse", 322, 445, Size.A3ExtraTransverse));
            All.Add(new Types("A3 rotated", 420, 297, Size.A3Rotated));
            All.Add(new Types("A3 transverse", 297, 420, Size.A3Transverse));
            All.Add(new Types("A4", 210, 297, Size.A4));
            All.Add(new Types("A4 extra", 236, 322, Size.A4Extra));
            All.Add(new Types("A4 plus", 210, 330, Size.A4Plus));
            All.Add(new Types("A4 rotated", 297, 210, Size.A4Rotated));
            All.Add(new Types("A4 small", 210, 297, Size.A4Small));
            All.Add(new Types("A4 transverse", 210, 297, Size.A4Transverse));
            All.Add(new Types("A5", 148, 210, Size.A5));
            All.Add(new Types("A5 extra", 174, 235, Size.A5Extra));
            All.Add(new Types("A5 rotated", 210, 148, Size.A5Rotated));
            All.Add(new Types("A5 transverse", 148, 210, Size.A5Transverse));
            All.Add(new Types("A6", 105, 148, Size.A6));
            All.Add(new Types("A6 rotated", 148, 105, Size.A6Rotated));
            All.Add(new Types("SuperA/SuperA/A4", 227, 356, Size.APlus));
            All.Add(new Types("B4", 250, 353, Size.B4));
            All.Add(new Types("B4 envelope", 250, 353, Size.B4Envelope));
            All.Add(new Types("JIS B4 rotated", 364, 257, Size.B4JisRotated));
            All.Add(new Types("B5", 176, 250, Size.B5));
            All.Add(new Types("B5 envelope", 176, 250, Size.B5Envelope));
            All.Add(new Types("ISO B5 extra", 201, 276, Size.B5Extra));
            All.Add(new Types("JIS B5 rotated", 257, 182, Size.B5JisRotated));
            All.Add(new Types("JIS B5 transverse", 182, 257, Size.B5Transverse));
            All.Add(new Types("B6 envelope", 176, 125, Size.B6Envelope));
            All.Add(new Types("JIS B6", 128, 182, Size.B6Jis));
            All.Add(new Types("JIS B6 rotated", 182, 128, Size.B6JisRotated));
            All.Add(new Types("SuperB/SuperB/A3", 305, 487, Size.BPlus));
            All.Add(new Types("C3 envelope", 324, 458, Size.C3Envelope));
            All.Add(new Types("C4 envelope", 229, 324, Size.C4Envelope));
            All.Add(new Types("C5 envelope", 162, 229, Size.C5Envelope));
            All.Add(new Types("C65 envelope", 114, 229, Size.C65Envelope));
            All.Add(new Types("C6 envelope", 114, 162, Size.C6Envelope));
            All.Add(new Types("C", 432, 559, Size.CSheet));
            All.Add(new Types("DL envelope", 110, 220, Size.DLEnvelope));
            All.Add(new Types("D", 559, 864, Size.DSheet));
            All.Add(new Types("E", 864, 1118, Size.ESheet));
            All.Add(new Types("Executive", 184, 267, Size.Executive));
            All.Add(new Types("Folio", 216, 330, Size.Folio));
            All.Add(new Types("German legal fanfold", 216, 330, Size.GermanLegalFanfold));
            All.Add(new Types("German standard fanfold", 216, 305, Size.GermanStandardFanfold));
            All.Add(new Types("Invitation envelope", 220, 220, Size.InviteEnvelope));
            All.Add(new Types("ISO B4", 250, 353, Size.IsoB4));
            All.Add(new Types("Italy envelope", 110, 230, Size.ItalyEnvelope));
            All.Add(new Types("Ledger", 432, 279, Size.Ledger));
            All.Add(new Types("Legal", 216, 356, Size.Legal));
            All.Add(new Types("Letter", 216, 279, Size.Letter));
            All.Add(new Types("Letter extra transverse", 236, 305, Size.LetterExtraTransverse));
            All.Add(new Types("Letter plus", 216, 322, Size.LetterPlus));
            All.Add(new Types("Letter rotated", 279, 216, Size.LetterRotated));
            All.Add(new Types("Letter small", 216, 279, Size.LetterSmall));
            All.Add(new Types("Letter transverse", 210, 279, Size.LetterTransverse));
            All.Add(new Types("Monarch envelope", 98, 191, Size.MonarchEnvelope));
            All.Add(new Types("Note", 216, 279, Size.Note));
            All.Add(new Types("#10 envelope", 105, 241, Size.Number10Envelope));
            All.Add(new Types("#11 envelope", 114, 264, Size.Number11Envelope));
            All.Add(new Types("#12 envelope", 121, 279, Size.Number12Envelope));
            All.Add(new Types("#14 envelope", 127, 292, Size.Number14Envelope));
            All.Add(new Types("#9 envelope", 98, 225, Size.Number9Envelope));
            All.Add(new Types("6 3/4 envelope", 92, 165, Size.PersonalEnvelope));
            All.Add(new Types("Quarto", 215, 275, Size.Quarto));
            All.Add(new Types("Standard", 254, 279, Size.Standard10x11));
            All.Add(new Types("Standard", 254, 356, Size.Standard10x14));
            All.Add(new Types("Standard", 279, 432, Size.Standard11x17));
            All.Add(new Types("Standard", 305, 279, Size.Standard12x11));
            All.Add(new Types("Standard", 381, 279, Size.Standard15x11));
            All.Add(new Types("Standard", 229, 279, Size.Standard9x11));
            All.Add(new Types("Statement", 140, 216, Size.Statement));
            All.Add(new Types("Tabloid", 279, 432, Size.Tabloid));
            All.Add(new Types("Tabloid extra", 297, 457, Size.TabloidExtra));
            All.Add(new Types("US standard fanfold", 378, 279, Size.USStandardFanfold));
        }

    //    public static Size Match(float widthPixel, float heightPixel, short tolerance = 5)
    //    {
    //        List<Type> lPossibleMatches = new List<Type>();
    //        try
    //        {
    //            int width = (int)Math.Round(widthPixel * 0.352777777777, 0);
    //            int height = (int)Math.Round(heightPixel * 0.352777777777, 0);

    //            lPossibleMatches.AddRange(
    //                All.Where(p =>
    //                (p.Height - tolerance) <= height && (p.Height + tolerance) >= height &&
    //                (p.Width - tolerance) <= width && (p.Width + tolerance) >= width)
    //                .ToList());

    //            if (lPossibleMatches.Count > 1)
    //            {
    //                Type Match = ReduceMultiMatch(lPossibleMatches, width, height, tolerance);
    //                return Match.Kind;
    //            }
    //            else if (lPossibleMatches.Count == 1)
    //            {
    //                return lPossibleMatches[0].Kind;
    //            }

    //            //Find matches for landscape orientation
    //            if (lPossibleMatches.Count == 0)
    //            {
    //                lPossibleMatches.AddRange(
    //                    All.Where(p =>
    //                    (p.Height - tolerance) <= width && (p.Height + tolerance) >= width &&
    //                    (p.Width - tolerance) <= height && (p.Width + tolerance) >= height)
    //                    .ToList());

    //                if (lPossibleMatches.Count > 1)
    //                {
    //                    Type Match = ReduceMultiMatch(lPossibleMatches, height, width, tolerance);
    //                }
    //                else if (lPossibleMatches.Count == 1)
    //                {
    //                    return lPossibleMatches[0].Kind;
    //                }
    //            }
    //        }
    //        catch (Exception ae)
    //        { string strError = ae.Message.ToString(); }
    //        return Size.Custom;
    //    }

    //    private static Type ReduceMultiMatch(List<Type> lMatches, int width, int height, short tolerance)
    //    {
    //        try
    //        {
    //            for (short i = 0; i < tolerance; i++)
    //            {
    //                if (All.Count(p =>
    //                    (p.Height - i) <= width && (p.Height + i) >= width &&
    //                    (p.Width - i) <= height && (p.Width + i) >= height) == 1)
    //                {
    //                    return All.First(p =>
    //                        (p.Height - i) <= width && (p.Height + i) >= width &&
    //                        (p.Width - i) <= height && (p.Width + i) >= height);
    //                }
    //                else if (All.Count(p =>
    //                    (p.Height - i) <= width && (p.Height + i) >= width &&
    //                    (p.Width - i) <= height && (p.Width + i) >= height) == 1)
    //                {
    //                    return All.First(p =>
    //                        (p.Height - i) <= width && (p.Height + i) >= width &&
    //                        (p.Width - i) <= height && (p.Width + i) >= height);
    //                }
    //            }
    //        }
    //        catch (Exception ae)
    //        { string strError = ae.Message.ToString(); }
    //        return lMatches[0];
    //    }
    }
}