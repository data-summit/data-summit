using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Methods.PostProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Methods.PostProcessing
{
    public class Self
    {
        public List<int> TotalTally = new List<int>();
        public List<Sentences> Clean(List<Sentences> transferOCR, short Tolerance = 9)
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

                string sTal = string.Join(Environment.NewLine, TotalTally.ToArray());
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }

        private List<Sentences> IntersectDeltas(List<Sentences> transferOCR)
        {
            List<Sentences> lOut = new List<Sentences>();
            try
            {
                //Merge differing size overlap sets and extract the differences
                List<Task> lOverTasks = new List<Task>();

                List<Sentences> l1 = new List<Sentences>();
                List<Sentences> l2 = new List<Sentences>();
                List<Sentences> l3 = new List<Sentences>();
                lOverTasks.Add(Task.Run(() =>
                {
                    l1 = MergeOverlaps(transferOCR.Select(x => x.Clone() as Sentences).ToList(), 5); TotalTally.Add(l1.Count(s => s.IsUsed == true));
                }));
                lOverTasks.Add(Task.Run(() =>
                {
                    l2 = MergeOverlaps(transferOCR.Select(x => x.Clone() as Sentences).ToList(), 15); TotalTally.Add(l2.Count(s => s.IsUsed == true));
                }));
                lOverTasks.Add(Task.Run(() =>
                {
                    l3 = MergeOverlaps(transferOCR.Select(x => x.Clone() as Sentences).ToList(), 25); TotalTally.Add(l3.Count(s => s.IsUsed == true));
                }));
                Task.WaitAll(lOverTasks.ToArray());

                List<Sentences> d12 = IsUsedChanges(l1, l2);
                List<Sentences> d23 = IsUsedChanges(l2, l3);
                List<Sentences> d13 = IsUsedChanges(l1, l3);

                List<Sentences> d1 = d13.Where(x => x.SlendernessRatio < 3).ToList();

                foreach (Sentences s in l1)
                {
                    foreach (Sentences ss in d1)
                    {
                        if (s.Height == ss.Height &&
                            s.Width == ss.Width &&
                            s.Left == ss.Left &&
                            s.Top == ss.Top &&
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

        private List<Sentences> IsUsedChanges(List<Sentences> l1, List<Sentences> l2)
        {
            List<Sentences> lOut = new List<Sentences>();
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

        private List<Sentences> ExactDuplicates(List<Sentences> transferOCR)
        {
            try
            {
                foreach (Sentences ss in transferOCR.ToList())
                {
                    if (ss.IsUsed == true)
                    {
                        if (transferOCR.Count(s =>
                                              s.Height == ss.Height &&
                                              s.Left == ss.Left &&
                                              s.Top == ss.Top &&
                                              s.Width == ss.Width &&
                                              s.IsUsed == true) > 1)
                        {
                            List<Sentences> l = transferOCR.Where(s => s.Height == ss.Height && s.Left == ss.Left &&
                                                   s.Top == ss.Top && s.Width == ss.Width &&
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

        private List<Sentences> NearDuplicates(List<Sentences> transferOCR, int Tolerance)
        {
            try
            {
                //transferOCR = transferOCR.Where(s => s.Words.Contains("2500")).ToList();
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentences ss = transferOCR[i];
                    List<Sentences> lCompare = transferOCR.Except(new List<Sentences> { ss }).ToList();
                    //.Where(s => s.IsUsed == true).ToList();

                    if (ss.IsUsed == true)
                    {
                        if (lCompare.Count(s =>
                                (s.Height <= (ss.Height + Tolerance)) &&
                                (s.Height >= (ss.Height - Tolerance)) &&
                                (s.Width <= (ss.Width + Tolerance)) &&
                                (s.Width >= (ss.Width - Tolerance)) &&
                                (s.Left <= (ss.Left + Tolerance)) &&
                                (s.Left >= (ss.Left - Tolerance)) &&
                                (s.Top <= (ss.Top + Tolerance)) &&
                                (s.Top >= (ss.Top - Tolerance)) &&
                                (s.IsUsed == true)) > 0)
                        {
                            List<Sentences> lPartialMatches = lCompare.Where(s =>
                                        (s.Height <= (ss.Height + Tolerance)) &&
                                        (s.Height >= (ss.Height - Tolerance)) &&
                                        (s.Width <= (ss.Width + Tolerance)) &&
                                        (s.Width >= (ss.Width - Tolerance)) &&
                                        (s.Left <= (ss.Left + Tolerance)) &&
                                        (s.Left >= (ss.Left - Tolerance)) &&
                                        (s.Top <= (ss.Top + Tolerance)) &&
                                        (s.Top >= (ss.Top - Tolerance)) &&
                                        (s.IsUsed == true)).ToList();

                            List<Sentences> l = PartialBox(ss, lPartialMatches); //Original combined with matches, to determine most appropriate

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

        private List<Sentences> MergeOverlaps(List<Sentences> transferOCR, int Tolerance)
        {
            List<Sentences> lOut = transferOCR.Select(x => x.Clone() as Sentences).ToList();
            try
            {
                for (int i = 0; i < lOut.Count; i++)
                {
                    Sentences ss = lOut[i];
                    List<Sentences> lCompare = lOut.Except(new List<Sentences> { ss }).ToList();

                    if (ss.IsUsed == true)
                    {
                        //Same horizontal start different end
                        if (lCompare.Count(s =>
                                (s.Height <= (ss.Height + Tolerance)) &&
                                (s.Height >= (ss.Height - Tolerance)) &&
                                (s.Left <= (ss.Left + Tolerance)) &&
                                (s.Left >= (ss.Left - Tolerance)) &&
                                (s.Top <= (ss.Top + Tolerance)) &&
                                (s.Top >= (ss.Top - Tolerance)) &&
                                (s.IsUsed == true)) > 0)
                        {
                            List<Sentences> lPartialMatches = lCompare.Where(s =>
                                        (s.Height <= (ss.Height + Tolerance)) &&
                                        (s.Height >= (ss.Height - Tolerance)) &&
                                        (s.Left <= (ss.Left + Tolerance)) &&
                                        (s.Left >= (ss.Left - Tolerance)) &&
                                        (s.Top <= (ss.Top + Tolerance)) &&
                                        (s.Top >= (ss.Top - Tolerance)) &&
                                        (s.IsUsed == true)).ToList();

                            List<Sentences> l = PartialBox(ss, lPartialMatches);
                            for (int j = 0; j < l.Count; j++)
                            { l[j].IsUsed = false; }
                        }

                        //Same horizontal end different start 
                        if (lCompare.Count(s =>
                                (s.Height <= (ss.Height + Tolerance)) &&
                                (s.Height >= (ss.Height - Tolerance)) &&
                                (s.Top <= (ss.Top + Tolerance)) &&
                                (s.Top >= (ss.Top - Tolerance)) &&
                                ((s.Left + s.Width) <= (ss.Left + ss.Width + Tolerance)) &&
                                ((s.Left + s.Width) >= (ss.Left + ss.Width - Tolerance)) &&
                                (s.IsUsed == true)) > 0)
                        {
                            List<Sentences> lPartialMatches = lCompare.Where(s =>
                                    (s.Height <= (ss.Height + Tolerance)) &&
                                    (s.Height >= (ss.Height - Tolerance)) &&
                                    (s.Top <= (ss.Top + Tolerance)) &&
                                    (s.Top >= (ss.Top - Tolerance)) &&
                                    ((s.Left + s.Width) <= (ss.Left + ss.Width + Tolerance)) &&
                                    ((s.Left + s.Width) >= (ss.Left + ss.Width - Tolerance)) &&
                                    (s.IsUsed == true)).ToList();

                            List<Sentences> l = PartialBox(ss, lPartialMatches);
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

        private List<Sentences> MergeIntersects(List<Sentences> transferOCR, int Tolerance)
        {
            try
            {
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentences ss = transferOCR[i];
                    List<Sentences> lCompare = transferOCR.Except(new List<Sentences> { ss }).ToList();

                    if (lCompare.Count(s =>
                         (s.Left <= (ss.Left + Tolerance)) &&
                         (s.Left >= (ss.Left - Tolerance)) &&
                         (s.Top <= (ss.Top + Tolerance)) &&
                         (s.Top >= (ss.Top - Tolerance)) &&
                         (s.Height <= (ss.Height + Tolerance)) &&
                         (s.Height >= (ss.Height - Tolerance)) &&
                         (s.IsUsed == true)) > 1)
                    {
                        List<Sentences> lPartialMatches = lCompare.Where(s =>
                            (s.Left <= (ss.Left + Tolerance)) &&
                            (s.Left >= (ss.Left - Tolerance)) &&
                            (s.Top <= (ss.Top + Tolerance)) &&
                            (s.Top >= (ss.Top - Tolerance)) &&
                            (s.Height <= (ss.Height + Tolerance)) &&
                            (s.Height >= (ss.Height - Tolerance)) &&
                            (s.IsUsed == true)).ToList();

                        List<Sentences> l = PartialBox(ss, lPartialMatches);
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

        private List<Sentences> Encapsulation(List<Sentences> transferOCR)
        {
            try
            {
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentences ss = transferOCR[i];
                    List<Sentences> lCompare = transferOCR.Except(new List<Sentences> { ss }).ToList();

                    if (ss.IsUsed == true)
                    {
                        if (lCompare.Count(s =>
                                (s.Left > ss.Left) &&
                                (s.Top > ss.Top) &&
                                ((s.Left + s.Width) < (ss.Left + ss.Width)) &&
                                ((s.Height + s.Top) < (ss.Height + ss.Top)) &&
                                (s.IsUsed == true)) > 1)
                        {
                            List<Sentences> lPartialMatches = lCompare.Where(s =>
                                        (s.Left > ss.Left) &&
                                        (s.Top > ss.Top) &&
                                        ((s.Left + s.Width) < (ss.Left + ss.Width)) &&
                                        ((s.Height + s.Top) < (ss.Height + ss.Top)) &&
                                        (s.IsUsed == true)).ToList();

                            List<Sentences> l = PartialBox(ss, lPartialMatches);
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

        private List<Sentences> PartialBox(Sentences ss, List<Sentences> lOthers)
        {
            List<Sentences> lsO = new List<Sentences>();
            try
            {
                //Determine if any others are wider than the current sentence
                if (lOthers.Count(s => s.Width > ss.Width) > 0)
                {
                    //Get others which maybe narrower, leaving the widest
                    lsO = lOthers.Where(s => s.Width < ss.Width).ToList();
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

        private List<Sentences> MergeNonSlenderOverlaps(List<Sentences> transferOCR, int Tolerance)
        {
            try
            {
                List<decimal> lRatios = transferOCR.Select(s => s.SlendernessRatio).ToList();
                decimal aveRatio = transferOCR.Average(s => s.SlendernessRatio);
                List<Sentences> lTargets = new List<Sentences>();
                for (int i = 0; i < transferOCR.Count; i++)
                {
                    Sentences ss = transferOCR[i];
                    List<Sentences> lCompare = transferOCR.Except(new List<Sentences> { ss }).ToList();

                    //Same horizontal start different end
                    if (lCompare.Count(s =>
                            (s.Height <= (ss.Height + Tolerance)) &&
                            (s.Height >= (ss.Height - Tolerance)) &&
                            (s.Left <= (ss.Left + Tolerance)) &&
                            (s.Left >= (ss.Left - Tolerance)) &&
                            (s.Top <= (ss.Top + Tolerance)) &&
                            (s.Top >= (ss.Top - Tolerance))) > 0)
                    {
                        List<Sentences> lPartialMatches = lCompare.Where(s =>
                                    (s.Height <= (ss.Height + Tolerance)) &&
                                    (s.Height >= (ss.Height - Tolerance)) &&
                                    (s.Left <= (ss.Left + Tolerance)) &&
                                    (s.Left >= (ss.Left - Tolerance)) &&
                                    (s.Top <= (ss.Top + Tolerance)) &&
                                    (s.Top >= (ss.Top - Tolerance))).ToList();

                        lTargets.AddRange(PartialBox(ss, lPartialMatches));
                    }

                    //Same horizontal end different start 
                    if (lCompare.Count(s =>
                            (s.Height <= (ss.Height + Tolerance)) &&
                            (s.Height >= (ss.Height - Tolerance)) &&
                            (s.Top <= (ss.Top + Tolerance)) &&
                            (s.Top >= (ss.Top - Tolerance)) &&
                            ((s.Left + s.Width) <= (ss.Left + ss.Width + Tolerance)) &&
                            ((s.Left + s.Width) >= (ss.Left + ss.Width - Tolerance))) > 0)
                    {
                        List<Sentences> lPartialMatches = lCompare.Where(s =>
                                (s.Height <= (ss.Height + Tolerance)) &&
                                (s.Height >= (ss.Height - Tolerance)) &&
                                (s.Top <= (ss.Top + Tolerance)) &&
                                (s.Top >= (ss.Top - Tolerance)) &&
                                ((s.Left + s.Width) <= (ss.Left + ss.Width + Tolerance)) &&
                                ((s.Left + s.Width) >= (ss.Left + ss.Width - Tolerance))).ToList();

                        lTargets.AddRange(PartialBox(ss, lPartialMatches));
                    }
                }

                List<Sentences> lDes1 = new List<Sentences>();
                List<Sentences> lDes2 = new List<Sentences>();
                foreach (Sentences s in lTargets)
                {
                    foreach (Sentences ss in transferOCR)
                    {
                        if (s.Equals(ss) && s.Vendor.Equals(ss.Vendor)
                            && s.Words.Equals(ss.Words) && s.IsUsed == ss.IsUsed)
                        { lDes1.Add(s); }
                        if (s.Equals(ss) && s.Vendor.Equals(ss.Vendor)
                            && s.Words.Equals(ss.Words) && s.IsUsed != ss.IsUsed)
                        { lDes2.Add(s); }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return transferOCR;
        }
    }
}