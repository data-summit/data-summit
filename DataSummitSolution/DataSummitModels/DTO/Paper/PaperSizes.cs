using DataSummitModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitModels.DTO.Paper
{
    // TODO: Get rid of this
    // Microsoft already has a class in System.Drawing.Printing with a proper name - PaperSize
    public static class PaperSizes
    {
        public static List<PaperType> All = new List<PaperType>();

        public static void Load()
        {
            All.Add(new PaperType("A0", 841, 1189, PaperSize.A0));
            All.Add(new PaperType("A1", 594, 841, PaperSize.A1));
            All.Add(new PaperType("A2", 420, 594, PaperSize.A2));
            All.Add(new PaperType("A3", 297, 420, PaperSize.A3));
            All.Add(new PaperType("A3 extra", 322, 445, PaperSize.A3Extra));
            All.Add(new PaperType("A3 extra transverse", 322, 445, PaperSize.A3ExtraTransverse));
            All.Add(new PaperType("A3 rotated", 420, 297, PaperSize.A3Rotated));
            All.Add(new PaperType("A3 transverse", 297, 420, PaperSize.A3Transverse));
            All.Add(new PaperType("A4", 210, 297, PaperSize.A4));
            All.Add(new PaperType("A4 extra", 236, 322, PaperSize.A4Extra));
            All.Add(new PaperType("A4 plus", 210, 330, PaperSize.A4Plus));
            All.Add(new PaperType("A4 rotated", 297, 210, PaperSize.A4Rotated));
            All.Add(new PaperType("A4 small", 210, 297, PaperSize.A4Small));
            All.Add(new PaperType("A4 transverse", 210, 297, PaperSize.A4Transverse));
            All.Add(new PaperType("A5", 148, 210, PaperSize.A5));
            All.Add(new PaperType("A5 extra", 174, 235, PaperSize.A5Extra));
            All.Add(new PaperType("A5 rotated", 210, 148, PaperSize.A5Rotated));
            All.Add(new PaperType("A5 transverse", 148, 210, PaperSize.A5Transverse));
            All.Add(new PaperType("A6", 105, 148, PaperSize.A6));
            All.Add(new PaperType("A6 rotated", 148, 105, PaperSize.A6Rotated));
            All.Add(new PaperType("SuperA/SuperA/A4", 227, 356, PaperSize.APlus));
            All.Add(new PaperType("B4", 250, 353, PaperSize.B4));
            All.Add(new PaperType("B4 envelope", 250, 353, PaperSize.B4Envelope));
            All.Add(new PaperType("JIS B4 rotated", 364, 257, PaperSize.B4JisRotated));
            All.Add(new PaperType("B5", 176, 250, PaperSize.B5));
            All.Add(new PaperType("B5 envelope", 176, 250, PaperSize.B5Envelope));
            All.Add(new PaperType("ISO B5 extra", 201, 276, PaperSize.B5Extra));
            All.Add(new PaperType("JIS B5 rotated", 257, 182, PaperSize.B5JisRotated));
            All.Add(new PaperType("JIS B5 transverse", 182, 257, PaperSize.B5Transverse));
            All.Add(new PaperType("B6 envelope", 176, 125, PaperSize.B6Envelope));
            All.Add(new PaperType("JIS B6", 128, 182, PaperSize.B6Jis));
            All.Add(new PaperType("JIS B6 rotated", 182, 128, PaperSize.B6JisRotated));
            All.Add(new PaperType("SuperB/SuperB/A3", 305, 487, PaperSize.BPlus));
            All.Add(new PaperType("C3 envelope", 324, 458, PaperSize.C3Envelope));
            All.Add(new PaperType("C4 envelope", 229, 324, PaperSize.C4Envelope));
            All.Add(new PaperType("C5 envelope", 162, 229, PaperSize.C5Envelope));
            All.Add(new PaperType("C65 envelope", 114, 229, PaperSize.C65Envelope));
            All.Add(new PaperType("C6 envelope", 114, 162, PaperSize.C6Envelope));
            All.Add(new PaperType("C", 432, 559, PaperSize.CSheet));
            All.Add(new PaperType("DL envelope", 110, 220, PaperSize.DLEnvelope));
            All.Add(new PaperType("D", 559, 864, PaperSize.DSheet));
            All.Add(new PaperType("E", 864, 1118, PaperSize.ESheet));
            All.Add(new PaperType("Executive", 184, 267, PaperSize.Executive));
            All.Add(new PaperType("Folio", 216, 330, PaperSize.Folio));
            All.Add(new PaperType("German legal fanfold", 216, 330, PaperSize.GermanLegalFanfold));
            All.Add(new PaperType("German standard fanfold", 216, 305, PaperSize.GermanStandardFanfold));
            All.Add(new PaperType("Invitation envelope", 220, 220, PaperSize.InviteEnvelope));
            All.Add(new PaperType("ISO B4", 250, 353, PaperSize.IsoB4));
            All.Add(new PaperType("Italy envelope", 110, 230, PaperSize.ItalyEnvelope));
            All.Add(new PaperType("Ledger", 432, 279, PaperSize.Ledger));
            All.Add(new PaperType("Legal", 216, 356, PaperSize.Legal));
            All.Add(new PaperType("Letter", 216, 279, PaperSize.Letter));
            All.Add(new PaperType("Letter extra transverse", 236, 305, PaperSize.LetterExtraTransverse));
            All.Add(new PaperType("Letter plus", 216, 322, PaperSize.LetterPlus));
            All.Add(new PaperType("Letter rotated", 279, 216, PaperSize.LetterRotated));
            All.Add(new PaperType("Letter small", 216, 279, PaperSize.LetterSmall));
            All.Add(new PaperType("Letter transverse", 210, 279, PaperSize.LetterTransverse));
            All.Add(new PaperType("Monarch envelope", 98, 191, PaperSize.MonarchEnvelope));
            All.Add(new PaperType("Note", 216, 279, PaperSize.Note));
            All.Add(new PaperType("#10 envelope", 105, 241, PaperSize.Number10Envelope));
            All.Add(new PaperType("#11 envelope", 114, 264, PaperSize.Number11Envelope));
            All.Add(new PaperType("#12 envelope", 121, 279, PaperSize.Number12Envelope));
            All.Add(new PaperType("#14 envelope", 127, 292, PaperSize.Number14Envelope));
            All.Add(new PaperType("#9 envelope", 98, 225, PaperSize.Number9Envelope));
            All.Add(new PaperType("6 3/4 envelope", 92, 165, PaperSize.PersonalEnvelope));
            All.Add(new PaperType("Quarto", 215, 275, PaperSize.Quarto));
            All.Add(new PaperType("Standard", 254, 279, PaperSize.Standard10x11));
            All.Add(new PaperType("Standard", 254, 356, PaperSize.Standard10x14));
            All.Add(new PaperType("Standard", 279, 432, PaperSize.Standard11x17));
            All.Add(new PaperType("Standard", 305, 279, PaperSize.Standard12x11));
            All.Add(new PaperType("Standard", 381, 279, PaperSize.Standard15x11));
            All.Add(new PaperType("Standard", 229, 279, PaperSize.Standard9x11));
            All.Add(new PaperType("Statement", 140, 216, PaperSize.Statement));
            All.Add(new PaperType("Tabloid", 279, 432, PaperSize.Tabloid));
            All.Add(new PaperType("Tabloid extra", 297, 457, PaperSize.TabloidExtra));
            All.Add(new PaperType("US standard fanfold", 378, 279, PaperSize.USStandardFanfold));
        }

        public static PaperSize Match(float widthPixel, float heightPixel, short tolerance = 5)
        {
            List<PaperType> lPossibleMatches = new List<PaperType>();
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
                    PaperType Match = ReduceMultiMatch(lPossibleMatches, width, height, tolerance);
                    return Match.Size;
                }
                else if (lPossibleMatches.Count == 1)
                {
                    return lPossibleMatches[0].Size;
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
                        PaperType Match = ReduceMultiMatch(lPossibleMatches, height, width, tolerance);
                    }
                    else if (lPossibleMatches.Count == 1)
                    {
                        return lPossibleMatches[0].Size;
                    }
                }
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return PaperSize.Custom;
        }

        private static PaperType ReduceMultiMatch(List<PaperType> lMatches, int width, int height, short tolerance)
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