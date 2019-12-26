using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Consolidated;

namespace TemplateMapper.OCR.PostProcessing
{
    public class SelfLocal
    {
        public List<int> TotalTally = new List<int>();
        public List<Sentence> Clean(List<Sentence> transferOCR, short Tolerance = 4)
        {
            List<Sentence> localOCR = new List<Sentence>();
            try
            {
                TotalTally.Add(transferOCR.Count);

                //All OCR attempts to consolidate OCR transferOCR need to be passed via this class
                localOCR = transferOCR;

                //Single source based on geometry only
                //Uses deductive methods from total list of sentences
                transferOCR = ExactDuplicates(transferOCR); TotalTally.Add(transferOCR.Count);
                transferOCR = NearDuplicates(transferOCR, Tolerance); TotalTally.Add(transferOCR.Count);
                transferOCR = MergeOverlaps(transferOCR, Tolerance); TotalTally.Add(transferOCR.Count);
                transferOCR = MergeIntersects(transferOCR, Tolerance); TotalTally.Add(transferOCR.Count);
                //transferOCR = Encapsulation(transferOCR); TotalTally.Add(transferOCR.Count);


                //List<Tuple<string, long, long, long, long, long, long>> tDelet = 
                //    SentenceToTuples(transferOCR.Sentences.Where(s => s.Words.Contains("scales")).ToList());
                //List<string> lWords = transferOCR.Sentences.Where(s => s.Words.Length > 16).Select(s => s.Words).ToList();
                //SentencesToCSV(transferOCR.Sentences.ToList());

                //transferOCR = TestMethodStruct(transferOCR);
                //Postfix & suffix attempt
                //Start with Microsoft only
                //Count number of occurcences that final (postfix) 3 letters in OCR rectangle 1 occur in OCR rectangle 2
                //Loop through list extending the postfix length ensuring that List.Count > 0 or SimMetrics similarity in 90%+
                //Once overlap range is determined, use midpoint string length overlap to created a new OCR rectangle with a new rectangle
                string sTal = string.Join(Environment.NewLine, TotalTally.ToArray());
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return transferOCR;
        }

        private List<Sentence> ExactDuplicates(List<Sentence> transferOCR)
        {
            List<Sentence> reducedOCR = new List<Sentence>();
            try
            {
                foreach (Sentence ss in transferOCR.ToList())
                {
                    if (reducedOCR.Count(s => s.Rectangle.Height == ss.Rectangle.Height && s.Rectangle.Left == ss.Rectangle.Left &&
                                       s.Rectangle.Top == ss.Rectangle.Top && s.Rectangle.Width == ss.Rectangle.Width) == 0)
                    { reducedOCR.Add(ss); }
                }

                //reducedOCR.FileName = transferOCR.FileName;
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return reducedOCR;
        }

        private List<Sentence> NearDuplicates(List<Sentence> transferOCR, short Tolerance = 4)
        {
            List<Sentence> reducedOCR = transferOCR;
            try
            {
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    if (lCompare.Count(s =>
                            (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                            (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                            (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                            (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                            (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                            (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance))
                            ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                                    (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                    (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                    (s.Rectangle.Width <= (ss.Rectangle.Width + Tolerance)) &&
                                    (s.Rectangle.Width >= (ss.Rectangle.Width - Tolerance)) &&
                                    (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                    (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                    (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                    (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance))
                                    ).ToList();

                        List<Tuple<string, long, long, long, long, long, long>> tRect = SentenceToTuples(lPartialMatches, ss);
                        List<Sentence> lToBeRemoved = PartialBox(ss, lPartialMatches);
                        foreach (Sentence sen in lToBeRemoved)
                        { reducedOCR.Remove(sen); }
                    }

                }

                //reducedOCR.FileName = transferOCR.FileName;
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return reducedOCR;
        }

        private List<Sentence> MergeOverlaps(List<Sentence> transferOCR, short Tolerance = 4)
        {
            Dictionary<int, string> vals = new Dictionary<int, string>();
            foreach(Sentence s in transferOCR)
            {
                if (s.Words.Contains("ROOF"))
                { vals.Add(transferOCR.IndexOf(s), s.Words); }
            }

            List<Sentence> reducedOCR = transferOCR;
            try
            {
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    //Same horizontal start different end
                    if (lCompare.Count(s =>
                            (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                            (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                            (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                            (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                            (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                            (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance))
                            ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                                    (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                    (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                    (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                    (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                    (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                    (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance))
                                    ).ToList();

                        List<Sentence> lToBeRemoved = PartialBox(ss, lPartialMatches);
                        //List<Tuple<string, long, long, long, long, long, long>> tRect = SentenceToTuples(lToBeRemoved);
                        foreach (Sentence sen in lToBeRemoved)
                        { reducedOCR.Remove(sen); }
                    }

                    //Same horizontal end different start 
                    if (lCompare.Count(s =>
                                (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) <= (ss.Rectangle.Left + ss.Rectangle.Width + Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) >= (ss.Rectangle.Left + ss.Rectangle.Width - Tolerance))
                                ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                                (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) <= (ss.Rectangle.Left + ss.Rectangle.Width + Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) >= (ss.Rectangle.Left + ss.Rectangle.Width - Tolerance))
                                ).ToList();

                        //List<Sentence> lToBeRemoved = PartialBox(reducedOCR, lPartialMatches);
                        List<Sentence> lToBeRemoved = PartialBox(ss, lPartialMatches);
                        //List<Tuple<string, long, long, long, long, long, long>> tRect = SentenceToTuples(lToBeRemoved);
                        foreach (Sentence sen in lToBeRemoved)
                        { reducedOCR.Remove(sen); }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            //reducedOCR.FileName = transferOCR.FileName;
            return reducedOCR;
        }

        private List<Sentence> MergeIntersects(List<Sentence> transferOCR, short Tolerance = 4)
        {
            List<Sentence> reducedOCR = transferOCR;
            try
            {
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    if (lCompare.Count(s =>
                         (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                         (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                         (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                         (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                         (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                         (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance))
                        ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                            (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                            (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                            (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                            (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                            (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                            (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance))
                            ).ToList();

                        List<Tuple<string, long, long, long, long, long, long>> tMatch = SentenceToTuples(lPartialMatches, ss);
                        List<Sentence> lToBeRemoved = PartialBox(ss, lPartialMatches);
                        List<Tuple<string, long, long, long, long, long, long>> tRemove = SentenceToTuples(lToBeRemoved);
                        foreach (Sentence sen in lToBeRemoved)
                        { reducedOCR.Remove(sen); }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            //reducedOCR.FileName = transferOCR.FileName;
            return reducedOCR;
        }

        private List<Sentence> Encapsulation(List<Sentence> transferOCR)
        {
            List<Sentence> reducedOCR = transferOCR;
            try
            {
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    if (lCompare.Count(s =>
                            (s.Rectangle.Left > ss.Rectangle.Left) &&
                            (s.Rectangle.Top > ss.Rectangle.Top) &&
                            ((s.Rectangle.Left + s.Rectangle.Width) < (ss.Rectangle.Left + ss.Rectangle.Width)) &&
                            ((s.Rectangle.Height + s.Rectangle.Top) < (ss.Rectangle.Height + ss.Rectangle.Top))
                            ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                                    (s.Rectangle.Left > ss.Rectangle.Left) &&
                                    (s.Rectangle.Top > ss.Rectangle.Top) &&
                                    ((s.Rectangle.Left + s.Rectangle.Width) < (ss.Rectangle.Left + ss.Rectangle.Width)) &&
                                    ((s.Rectangle.Height + s.Rectangle.Top) < (ss.Rectangle.Height + ss.Rectangle.Top))
                                    ).ToList();

                        List<Tuple<string, long, long, long, long, long, long>> tRect = SentenceToTuples(lPartialMatches, ss);
                        List<Sentence> lToBeRemoved = PartialBox(ss, lPartialMatches);
                        foreach (Sentence sen in lToBeRemoved)
                        { reducedOCR.Remove(sen); }
                    }

                }

                //reducedOCR.FileName = transferOCR.FileName;
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return reducedOCR;
        }

        private List<Sentence> PartialBox(Sentence ss, List<Sentence> lOthers)
        {
            List<Sentence> lsO = new List<Sentence>();
            try
            {
                //Determine if any others are wider than the current sentence
                if (lOthers.Count(s => s.Rectangle.Width > ss.Rectangle.Width) > 0)
                {
                    //Get others which maybe narrower, leaving the widest
                    lsO = lOthers.Where(s => s.Rectangle.Width < ss.Rectangle.Width).ToList();
                    lsO.Add(ss);
                }
                else    //Otherwise the current sentence is the widest
                {
                    lsO = lOthers.ToList();
                }
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return lsO;
        }

        private List<Tuple<string, long, long, long, long, long, long>> SentenceToTuples(List<Sentence> transferOCR, List<int> indexs)
        {
            List<Tuple<string, long, long, long, long, long, long>> tRes = new List<Tuple<string, long, long, long, long, long, long>>();
            foreach (int i in indexs)
            {
                Sentence ss = transferOCR[i];
                tRes.Add(Tuple.Create(ss.Words.ToString(), ss.Rectangle.Left, ss.Rectangle.Top, ss.Rectangle.Width, ss.Rectangle.Height,
                                      ss.Rectangle.Left + ss.Rectangle.Width, ss.Rectangle.Top + ss.Rectangle.Height));
            }
            return tRes;
        }

        private List<Tuple<string, long, long, long, long, long, long>> SentenceToTuples(List<Sentence> lsentences, Sentence s)
        {
            List<Tuple<string, long, long, long, long, long, long>> tRes = new List<Tuple<string, long, long, long, long, long, long>>();
            tRes.Add(Tuple.Create(s.Words.ToString(), s.Rectangle.Left, s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height,
                                      s.Rectangle.Left + s.Rectangle.Width, s.Rectangle.Top + s.Rectangle.Height));
            foreach (Sentence ss in lsentences)
            {
                tRes.Add(Tuple.Create(ss.Words.ToString(), ss.Rectangle.Left, ss.Rectangle.Top, ss.Rectangle.Width, ss.Rectangle.Height,
                                      ss.Rectangle.Left + ss.Rectangle.Width, ss.Rectangle.Top + ss.Rectangle.Height));
            }
            return tRes;
        }

        private List<Tuple<string, long, long, long, long, long, long>> SentenceToTuples(List<Sentence> lsentences)
        {
            List<Tuple<string, long, long, long, long, long, long>> tRes = new List<Tuple<string, long, long, long, long, long, long>>();
            foreach (Sentence ss in lsentences)
            {
                tRes.Add(Tuple.Create(ss.Words.ToString(), ss.Rectangle.Left, ss.Rectangle.Top, ss.Rectangle.Width, ss.Rectangle.Height,
                                      ss.Rectangle.Left + ss.Rectangle.Width, ss.Rectangle.Top + ss.Rectangle.Height));
            }
            return tRes;
        }

    }
}
