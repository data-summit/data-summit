using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Consolidated;

namespace TemplateMapper.OCR.PostProcessing
{
    public class Self
    {
        public List<int> TotalTally = new List<int>();
        public List<Sentence> Clean(List<Sentence> transferOCR, short Tolerance = 9)
        {
            try
            {
                TotalTally.Add(transferOCR.Count);

                transferOCR = ExactDuplicates(transferOCR); TotalTally.Add(transferOCR.Count(s => s.IsUsed == true));
                transferOCR = NearDuplicates(transferOCR, Tolerance); TotalTally.Add(transferOCR.Count(s => s.IsUsed == true));
                transferOCR = IntersectDeltas(transferOCR);
                transferOCR = MergeIntersects(transferOCR, Tolerance); TotalTally.Add(transferOCR.Count(s => s.IsUsed == true));
                transferOCR = Encapsulation(transferOCR); TotalTally.Add(transferOCR.Count(s => s.IsUsed == true));
                transferOCR = MergeNonSlenderOverlaps(transferOCR, 25); TotalTally.Add(transferOCR.Count(s => s.IsUsed == true));

                List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> lCustom = //SentenceToTuples(transferOCR);
                        SentenceToTuples(transferOCR.Where(s => s.Words.Equals("AIN") || s.Words.Equals("COND DRAIN")).ToList());
                List<Sentence> lRatioCheck = transferOCR.Where(s => s.Words.Equals("AIN") || s.Words.Equals("COND DRAIN")).ToList();
                string sTal = string.Join(Environment.NewLine, TotalTally.ToArray());
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }

        private List<Sentence> IntersectDeltas(List<Sentence> transferOCR)
        {
            List<Sentence> lOut = new List<Sentence>();
            try
            {
                //Merge differing size overlap sets and extract the differences
                List<Task> lOverTasks = new List<Task>();

                List<Sentence> l1 = new List<Sentence>();
                List<Sentence> l2 = new List<Sentence>();
                List<Sentence> l3 = new List<Sentence>();
                lOverTasks.Add(Task.Run(() =>
                {
                    l1 = MergeOverlaps(transferOCR.Select(x => x.Clone() as Sentence).ToList(), 5); TotalTally.Add(l1.Count(s => s.IsUsed == true));
                }));
                lOverTasks.Add(Task.Run(() =>
                {
                    l2 = MergeOverlaps(transferOCR.Select(x => x.Clone() as Sentence).ToList(), 15); TotalTally.Add(l2.Count(s => s.IsUsed == true));
                }));
                lOverTasks.Add(Task.Run(() =>
                {
                    l3 = MergeOverlaps(transferOCR.Select(x => x.Clone() as Sentence).ToList(), 25); TotalTally.Add(l3.Count(s => s.IsUsed == true));
                }));
                Task.WaitAll(lOverTasks.ToArray());

                List<Sentence> d12 = IsUsedChanges(l1, l2);
                List<Sentence> d23 = IsUsedChanges(l2, l3);
                List<Sentence> d13 = IsUsedChanges(l1, l3);

                List<Sentence> d1 = d13.Where(x => x.SlendernessRatio < 3).ToList();

                foreach(Sentence s in l1)
                {
                    foreach(Sentence ss in d1)
                    {
                        if (s.Rectangle.Height == ss.Rectangle.Height &&
                            s.Rectangle.Width == ss.Rectangle.Width &&
                            s.Rectangle.Left == ss.Rectangle.Left &&
                            s.Rectangle.Top == ss.Rectangle.Top &&
                            s.Words == ss.Words &&
                            s.Vendor == ss.Vendor)
                        {
                            s.IsUsed = ss.IsUsed;
                        }
                    }
                }
                //List<double> sl12 = d12.Select(x => x.SlendernessRatio).ToList();
                //List<double> sl23 = d23.Select(x => x.SlendernessRatio).ToList();
                //List<double> sl13 = d13.Select(x => x.SlendernessRatio).ToList();

                //List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> t12 = SentenceToTuples(d12);
                //List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> t23 = SentenceToTuples(d12);
                //List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> t13 = SentenceToTuples(d12);
                lOut = l1;
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return lOut;
        }

        private List<Sentence> IsUsedChanges(List<Sentence> l1, List<Sentence> l2)
        {
            List<Sentence> lOut = new List<Sentence>();
            try
            {
                for (int i = 0; i < l1.Count; i++)
                {
                    if (l1[i].IsUsed != l2[i].IsUsed)
                    {
                        lOut.Add(l2[i]);
                    }
                }
                //foreach (Sentence s in l1)
                //{
                //    foreach (Sentence ss in l2)
                //    {
                //        if (s.Rectangle.Height == ss.Rectangle.Height &&
                //            s.Rectangle.Left == ss.Rectangle.Left &&
                //            s.Rectangle.Top == ss.Rectangle.Top &&
                //            s.Rectangle.Width == ss.Rectangle.Width &&
                //            s.IsUsed != ss.IsUsed)
                //        { lOut.Add(ss); }
                //    }
                //}
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return lOut;
        }

        private List<Sentence> ExactDuplicates(List<Sentence> transferOCR)
        {
            try
            {
                foreach (Sentence ss in transferOCR.ToList())
                {
                    if (ss.IsUsed == true)
                    {
                        if (transferOCR.Count(s =>
                                              s.Rectangle.Height == ss.Rectangle.Height &&
                                              s.Rectangle.Left == ss.Rectangle.Left &&
                                              s.Rectangle.Top == ss.Rectangle.Top &&
                                              s.Rectangle.Width == ss.Rectangle.Width &&
                                              s.IsUsed == true) > 1)
                        {
                            List<Sentence> l = transferOCR.Where(s => s.Rectangle.Height == ss.Rectangle.Height && s.Rectangle.Left == ss.Rectangle.Left &&
                                                   s.Rectangle.Top == ss.Rectangle.Top && s.Rectangle.Width == ss.Rectangle.Width &&
                                                   s.IsUsed == true).ToList();

                            for (int j = 1; j < l.Count; j++)
                            { l[j].IsUsed = false; }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }

        private List<Sentence> NearDuplicates(List<Sentence> transferOCR, int Tolerance)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tAll = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            try
            {
                //transferOCR = transferOCR.Where(s => s.Words.Contains("2500")).ToList();
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    tAll = SentenceToTuples(transferOCR);
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();
                        //.Where(s => s.IsUsed == true).ToList();
                    
                    if (ss.IsUsed == true)
                    {
                        if (lCompare.Count(s =>
                                (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                (s.Rectangle.Width <= (ss.Rectangle.Width + Tolerance)) &&
                                (s.Rectangle.Width >= (ss.Rectangle.Width - Tolerance)) &&
                                (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                (s.IsUsed == true)) > 0)
                        {
                            List<Sentence> lPartialMatches = lCompare.Where(s =>
                                        (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                        (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                        (s.Rectangle.Width <= (ss.Rectangle.Width + Tolerance)) &&
                                        (s.Rectangle.Width >= (ss.Rectangle.Width - Tolerance)) &&
                                        (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                        (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                        (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                        (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                        (s.IsUsed == true)).ToList();

                            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tRect = SentenceToTuples(lPartialMatches, ss);
                            List<Sentence> l = PartialBox(ss, lPartialMatches); //Original combined with matches, to determine most appropriate
                                                                                //List<Sentence> l = PartialBox(lPartialMatches);
                            for (int j = 0; j < l.Count; j++)
                            { l[j].IsUsed = false; }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }

        private List<Sentence> MergeOverlaps(List<Sentence> transferOCR, int Tolerance)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tAll = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            List<Sentence> lOut = transferOCR.Select(x => x.Clone() as Sentence).ToList();
            try
            {
                //lOut = lOut.Where(s => s.Words.Contains("125")).ToList();
                for (int i = 0; i < lOut.Count; i++)
                {
                    tAll = SentenceToTuples(lOut.ToList());
                    Sentence ss = lOut[i];
                    List<Sentence> lCompare = lOut.Except(new List<Sentence> { ss }).ToList();

                    if (ss.IsUsed == true)
                    {
                        //Same horizontal start different end
                        if (lCompare.Count(s =>
                                (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                (s.IsUsed == true)) > 0)
                        {
                            List<Sentence> lPartialMatches = lCompare.Where(s =>
                                        (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                        (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                        (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                        (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                        (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                        (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                        (s.IsUsed == true)).ToList();

                            List<Sentence> l = PartialBox(ss, lPartialMatches);
                            for (int j = 0; j < l.Count; j++)
                            { l[j].IsUsed = false; }
                        }

                        //Same horizontal end different start 
                        if (lCompare.Count(s =>
                                (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) <= (ss.Rectangle.Left + ss.Rectangle.Width + Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) >= (ss.Rectangle.Left + ss.Rectangle.Width - Tolerance)) &&
                                (s.IsUsed == true)) > 0)
                        {
                            List<Sentence> lPartialMatches = lCompare.Where(s =>
                                    (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                    (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                    (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                    (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                    ((s.Rectangle.Left + s.Rectangle.Width) <= (ss.Rectangle.Left + ss.Rectangle.Width + Tolerance)) &&
                                    ((s.Rectangle.Left + s.Rectangle.Width) >= (ss.Rectangle.Left + ss.Rectangle.Width - Tolerance)) &&
                                    (s.IsUsed == true)).ToList();
                            //List<int> indexs = new List<int>();
                            //lPartialMatches.ForEach(s => indexs.Add(transferOCR.IndexOf(s)));

                            List<Sentence> l = PartialBox(ss, lPartialMatches);
                            for (int j = 0; j < l.Count; j++)
                            { l[j].IsUsed = false; }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lOut;
        }

        private List<Sentence> MergeIntersects(List<Sentence> transferOCR, int Tolerance)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tAll = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            try
            {
                //transferOCR = transferOCR.Where(s => s.Words.Contains("125")).ToList();
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    tAll = SentenceToTuples(transferOCR);
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    if (lCompare.Count(s =>
                         (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                         (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                         (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                         (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                         (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                         (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                         (s.IsUsed == true)) > 1)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                            (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                            (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                            (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                            (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                            (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                            (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                            (s.IsUsed == true)).ToList();

                        List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tPart = SentenceToTuples(lPartialMatches, ss);
                        List <Sentence> l = PartialBox(ss, lPartialMatches);
                        for (int j = 1; j < l.Count; j++)
                        { l[j].IsUsed = false; }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }

        private List<Sentence> Encapsulation(List<Sentence> transferOCR)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tAll = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            try
            {
                //transferOCR = transferOCR.Where(s => s.Words.Contains("I.V")).ToList();
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    tAll = SentenceToTuples(transferOCR);
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    if (ss.IsUsed == true)
                    {
                        if (lCompare.Count(s =>
                                (s.Rectangle.Left > ss.Rectangle.Left) &&
                                (s.Rectangle.Top > ss.Rectangle.Top) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) < (ss.Rectangle.Left + ss.Rectangle.Width)) &&
                                ((s.Rectangle.Height + s.Rectangle.Top) < (ss.Rectangle.Height + ss.Rectangle.Top)) &&
                                (s.IsUsed == true)) > 1)
                        {
                            List<Sentence> lPartialMatches = lCompare.Where(s =>
                                        (s.Rectangle.Left > ss.Rectangle.Left) &&
                                        (s.Rectangle.Top > ss.Rectangle.Top) &&
                                        ((s.Rectangle.Left + s.Rectangle.Width) < (ss.Rectangle.Left + ss.Rectangle.Width)) &&
                                        ((s.Rectangle.Height + s.Rectangle.Top) < (ss.Rectangle.Height + ss.Rectangle.Top)) &&
                                        (s.IsUsed == true)).ToList();

                            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tRect = SentenceToTuples(lPartialMatches, ss);
                            List<Sentence> l = PartialBox(ss, lPartialMatches);
                            for (int j = 1; j < l.Count; j++)
                            { l[j].IsUsed = false; }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
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
                string strError = ae.Message.ToString();
            }
            return lsO;
        }

        private List<Sentence> MergeNonSlenderOverlaps(List<Sentence> transferOCR, int Tolerance)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tAll = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            try
            {
                List<double> lRatios = transferOCR.Select(s => s.SlendernessRatio).ToList();
                double aveRatio = transferOCR.Average(s => s.SlendernessRatio);
                List<Sentence> lTargets = new List<Sentence>();
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    tAll = SentenceToTuples(transferOCR);
                    Sentence ss = transferOCR[i];
                    List<Sentence> lCompare = transferOCR.Except(new List<Sentence> { ss }).ToList();

                    //Same horizontal start different end
                    if (lCompare.Count(s =>
                            (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                            (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                            (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                            (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                            (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                            (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                                    (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                    (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                    (s.Rectangle.Left <= (ss.Rectangle.Left + Tolerance)) &&
                                    (s.Rectangle.Left >= (ss.Rectangle.Left - Tolerance)) &&
                                    (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                    (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) ).ToList();

                        lTargets.AddRange(PartialBox(ss, lPartialMatches));
                    }

                    //Same horizontal end different start 
                    if (lCompare.Count(s =>
                            (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                            (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                            (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                            (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                            ((s.Rectangle.Left + s.Rectangle.Width) <= (ss.Rectangle.Left + ss.Rectangle.Width + Tolerance)) &&
                            ((s.Rectangle.Left + s.Rectangle.Width) >= (ss.Rectangle.Left + ss.Rectangle.Width - Tolerance)) ) > 0)
                    {
                        List<Sentence> lPartialMatches = lCompare.Where(s =>
                                (s.Rectangle.Height <= (ss.Rectangle.Height + Tolerance)) &&
                                (s.Rectangle.Height >= (ss.Rectangle.Height - Tolerance)) &&
                                (s.Rectangle.Top <= (ss.Rectangle.Top + Tolerance)) &&
                                (s.Rectangle.Top >= (ss.Rectangle.Top - Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) <= (ss.Rectangle.Left + ss.Rectangle.Width + Tolerance)) &&
                                ((s.Rectangle.Left + s.Rectangle.Width) >= (ss.Rectangle.Left + ss.Rectangle.Width - Tolerance)) ).ToList();

                        lTargets.AddRange(PartialBox(ss, lPartialMatches));
                    }
                }

                List<Sentence> lDes1 = new List<Sentence>();
                List<Sentence> lDes2 = new List<Sentence>();
                foreach (Sentence s in lTargets)
                {
                    foreach(Sentence ss in transferOCR)
                    {
                        if (s.Rectangle.Equals(ss.Rectangle) && s.Vendor.Equals(ss.Vendor)
                            && s.Words.Equals(ss.Words) && s.IsUsed == ss.IsUsed)
                        { lDes1.Add(s); }
                        if (s.Rectangle.Equals(ss.Rectangle) && s.Vendor.Equals(ss.Vendor)
                            && s.Words.Equals(ss.Words) && s.IsUsed != ss.IsUsed)
                        { lDes2.Add(s); }
                    }
                }

                //List<Sentence> lIntersect1 = transferOCR.Where(f => lTargets.Equals(f)).ToList();
                //List<Sentence> lIntersect2 = transferOCR.Where(f => !lTargets.Equals(f)).ToList();
                
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }

        private List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> SentenceToTuples(List<Sentence> lsentences, Sentence s)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tRes = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            tRes.Add(Tuple.Create(s.Words.ToString(), s.IsUsed, s.Rectangle.Left, s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height,
                                      s.Rectangle.Left + s.Rectangle.Width, s.Rectangle.Top + s.Rectangle.Height));
            foreach (Sentence ss in lsentences)
            {
                tRes.Add(Tuple.Create(ss.Words.ToString(), ss.IsUsed, ss.Rectangle.Left, ss.Rectangle.Top, ss.Rectangle.Width, ss.Rectangle.Height,
                                      ss.Rectangle.Left + ss.Rectangle.Width, ss.Rectangle.Top + ss.Rectangle.Height));
            }
            tRes = tRes.OrderBy(i => i.Item1).ThenBy(i => i.Item2).ThenBy(i => i.Item3).ThenBy(i => i.Item4).ThenBy(i => i.Item5).ThenBy(i => i.Item6).ToList();
            return tRes;
        }

        private List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> SentenceToTuples(List<Sentence> lsentences)
        {
            List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>> tRes = new List<Tuple<string, bool, long, long, long, long, long, Tuple<long>>>();
            foreach (Sentence ss in lsentences)
            {
                tRes.Add(Tuple.Create(ss.Words.ToString(), ss.IsUsed, ss.Rectangle.Left, ss.Rectangle.Top, ss.Rectangle.Width, ss.Rectangle.Height,
                                      ss.Rectangle.Left + ss.Rectangle.Width, ss.Rectangle.Top + ss.Rectangle.Height));
            }
            tRes = tRes.OrderBy(i => i.Item1).ThenBy(i => i.Item3).ThenBy(i => i.Item4).ThenBy(i => i.Item5).ThenBy(i => i.Item6).ThenBy(i => i.Item2).ToList();
            return tRes;
        }

    }
}
