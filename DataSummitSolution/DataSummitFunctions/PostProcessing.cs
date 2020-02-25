using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Methods.PostProcessing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MoreLinq;
using Newtonsoft.Json;
using SimMetricsMetricUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitFunctions.Models.Recognition;

namespace DataSummitFunctions
{
    public static class PostProcessing
    {
        [FunctionName("PostProcessing")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                            HttpRequestMessage req, TraceWriter log)// Microsoft.Extensions.Logging.ILogger log)
        {
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<List<Sentences>>(jsonContent);
                List<Sentences> lFeatures = (List<Sentences>)data;
                List<RectanglePairs> lResults = new List<RectanglePairs>();
                


                string jsonToReturn = JsonConvert.SerializeObject(lFeatures);

                return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json") };
            }
            catch (Exception ae)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                { Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json") };
            }
        }
        private static Models.Drawing OCRAnomalies(Models.Drawing results)
        {
            Models.Drawing reducedOCR = new Models.Drawing();
            try
            {
                List<Sentences> lSameRectAndWords = new List<Sentences>();
                List<Sentences> lSameRectDifferentWords = new List<Sentences>();

                //foreach (Sentences ss in results.Sentences.ToList())
                //{
                //    if (lSameRectAndWords.Count(s => s.Height == ss.Height && s.Left == ss.Left &&
                //                       s.Top == ss.Top && s.Width == ss.Width &&
                //                       s.Words == ss.Words) == 0)
                //    { lSameRectAndWords.Add(ss); }
                //    if (lSameRectDifferentWords.Count(s => s.Height == ss.Height && s.Left == ss.Left &&
                //                       s.Top == ss.Top && s.Width == ss.Width &&
                //                       s.Words.ToString() != ss.Words.ToString()) == 0)
                //    { lSameRectDifferentWords.Add(ss); }
                //}

                ////Word anomalies, should be used for supervised learning
                //reducedOCR.Sentences = lSameRectAndWords.Except(lSameRectDifferentWords).ToList();
                ////reducedOCR.FileName = results.FileName;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return reducedOCR;
        }

        private static Models.Drawing MergeOverlaps(Models.Drawing results)
        {
            Models.Drawing c = new Models.Drawing();
            try
            {
                List<Sentences> lToBeDeleted = new List<Sentences>();
                List<Sentences> lToBeAdded = new List<Sentences>();
                List<Sentences> lMerged = new List<Sentences>();

                foreach (Models.Consolidated.Sentences sen in results.Sentences.Select(s => s.ToModelConsolidated()).ToList())
                {
                    List<Models.Consolidated.Sentences> ls2 = results.Sentences
                                                                .Where(s => !s.Equals(sen))
                                                                .Select(s => s.ToModelConsolidated())
                                                                .ToList();

                    List<string> s2 = ls2.Select(s => s.Words).ToList();
                    string s1 = sen.Words;
                    string se = sen.Words.Substring(sen.Words.Length - 4, 3);
                    List<Models.Consolidated.Sentences> s4 = ls2.Where(s => s.Words.Contains(se)).ToList();
                    if (s4.Count > 0)
                    {
                        foreach (Models.Consolidated.Sentences sen1 in s4)
                        {
                            if (ExactSuffixOrPostfix(sen, sen1) == false)
                            {
                                List<int> starts = sen1.Words.AllIndexesOf(se);
                                int iEarlistPos = -1; int isen1 = -1; int isen2 = -1;
                                foreach (int i in starts)
                                {
                                    string s3 = "";
                                    for (int j = sen.Words.Length - 5; j >= 0; j--)
                                    {
                                        string sen2 = sen.Words.Substring(j, sen.Words.Length - j - 1);
                                        s3 = sen1.Words.Substring(j, sen.Words.Length - j - 1);
                                        if (sen1.Words.Contains(sen.Words.Substring(j, sen.Words.Length - j - 1)))
                                        {
                                            iEarlistPos = j;
                                        }
                                        else
                                        {
                                            isen1 = sen1.Words.IndexOf(s3);
                                            isen2 = sen.Words.IndexOf(sen2);
                                            break;
                                        }
                                    }

                                    if (iEarlistPos == 0)     //Both strings start the same
                                    {
                                        //Delete the short sentence string
                                        lToBeDeleted.Add(sen);
                                        lToBeAdded.Add(sen1);
                                    }
                                    else
                                    {
                                        Sentences s = CharComparison(sen, sen1);
                                        if (s != null)
                                        { lToBeAdded.Add(s); }
                                    }
                                }
                                // iEarlistPos vs index of other, get mid point between both, split both string and concatenate
                            }
                        }
                    }
                    //Remove all confirmed deletes
                }
                lMerged = results.Sentences.Select(s => s.ToModelConsolidated()).ToList()
                                 .Intersect(lToBeDeleted.Select(s => s)).ToList();
                // Methods.PostProcessing.MultipleVendors.lResults.AddRange(lToBeAdded);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            //c.FileName = results.FileName;
            return c;
        }

        private static Sentences CharComparison(Sentences sCur, Sentences sTarget)
        {
            try
            {
                //Find shorter sentence
                Sentences sShort = null; Sentences sLonger = null;
                if (sCur.Words.Length < sTarget.Words.Length)
                {
                    sShort = sCur;
                    sLonger = sTarget;
                }
                else
                {
                    sShort = sTarget;
                    sLonger = sCur;
                }

                if (sShort.Words.Length > 4)
                {
                    List<Tuple<long, short, string, string>> tFromStart = new List<Tuple<long, short, string, string>>();
                    List<Tuple<long, short, string, string>> tFromEnd = new List<Tuple<long, short, string, string>>();

                    //Drop last 2 characters incase of boundary splitting
                    for (int i = 0; i < sShort.Words.Length - 2; i++)
                    {
                        string s1 = sShort.Words.Substring(i, 1);
                        string s2 = sLonger.Words.Substring(i, 1);
                        if (s1 == s2)
                        { tFromStart.Add(Tuple.Create((long)i, (short)1, s1, s2)); }
                        else
                        { tFromStart.Add(Tuple.Create((long)i, (short)0, s1, s2)); }
                    }

                    for (int i = sShort.Words.Length - 1; i >= 2; i--)
                    {
                        string s1 = sShort.Words.Substring(i, 1);
                        string s2 = sLonger.Words.Substring(i, 1);
                        if (s1 == s2)
                        { tFromEnd.Add(Tuple.Create((long)i, (short)1, s1, s2)); }
                        else
                        { tFromEnd.Add(Tuple.Create((long)i, (short)0, s1, s2)); }
                    }

                    double dblAveStart = tFromStart.Average(v => v.Item2);
                    double dblAveEnd = tFromEnd.Average(v => v.Item2);

                    SimMetricsMetricUtilities.SmithWaterman sw = new SmithWaterman();
                    List<double> ForwardAverage1 = new List<double>();
                    List<double> BackwardAverage1 = new List<double>();
                    List<double> ForwardAverage2 = new List<double>();
                    List<double> BackwardAverage2 = new List<double>();

                    List<bool> ForwardConverge = new List<bool>();
                    List<bool> BackwardConverge = new List<bool>();

                    for (int i = 1; i < tFromStart.Count; i++)
                    {
                        ForwardAverage1.Add(tFromStart.GetRange(0, i).Average(s => s.Item2));
                        BackwardAverage1.Add(tFromEnd.GetRange(0, i).Average(s => s.Item2));
                        string ss1 = String.Concat(tFromEnd.GetRange(0, i).Select(s => s.Item3));
                        string ss2 = String.Concat(tFromEnd.GetRange(0, i).Select(s => s.Item4));
                        ForwardAverage2.Add(sw.GetSimilarity(
                                        String.Concat(tFromStart.GetRange(0, i).Select(s => s.Item3)),
                                        String.Concat(tFromStart.GetRange(0, i).Select(s => s.Item4))));

                        BackwardAverage2.Add(sw.GetSimilarity(ss1, ss2));
                        //String.Concat(tFromEnd.GetRange(0, i).Select(s => s.Item3)),
                        //String.Concat(tFromEnd.GetRange(0, i).Select(s => s.Item4))));
                    }

                    for (int i = 0; i < tFromStart.Count - 2; i++)
                    {
                        if (ForwardAverage2[i] <= ForwardAverage2[i + 1])
                        { ForwardConverge.Add(true); }
                        else
                        { ForwardConverge.Add(false); }

                        if (BackwardAverage2[i] <= BackwardAverage2[i + 1])
                        { BackwardConverge.Add(true); }
                        else
                        { BackwardConverge.Add(false); }
                    }

                    //Determines failure location as opposed to an anomaly
                    List<bool> lFailCriteria = new List<bool>() { false, false, true, true, false };
                    int subListMatchIndex = ForwardConverge.SubListIndex(0, lFailCriteria);
                    if (subListMatchIndex > 3)
                    { return sShort; }

                    return sLonger;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return null;
        }

        /// <summary>
        /// Tests for a perfect match between both sentences, removing the shorter one and adds
        /// longer one to the 'lResults" public List()
        /// </summary>
        /// <param name="sCur"></param>
        /// <param name="sTarget"></param>
        /// <returns></returns>
        private static bool ExactSuffixOrPostfix(Models.Consolidated.Sentences sCur, Models.Consolidated.Sentences sTarget)
        {
            try
            {
                if (sCur.Words.IndexOf(sTarget.Words) == 0)
                {
                    //Methods.PostProcessing.MultipleVendors.lResults.Add(sCur);
                    return true;
                }
                if (sTarget.Words.IndexOf(sCur.Words) == 0)
                {
                    //Methods.PostProcessing.MultipleVendors.lResults.Add(sTarget);
                    return true;
                }
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return false;
        }

        private static List<int> AllIndexesOf(this string str, string value)
        {
            List<int> indexes = new List<int>();
            if (!String.IsNullOrEmpty(value))
            {
                for (int index = 0; ; index += value.Length)
                {
                    index = str.IndexOf(value, index);
                    if (index == -1)
                        return indexes;
                    indexes.Add(index);
                }
            }
            return indexes;
        }

        private static int SubListIndex<T>(this IList<T> list, int start, IList<T> sublist)
        {
            for (int listIndex = start; listIndex < list.Count - sublist.Count + 1; listIndex++)
            {
                int count = 0;
                while (count < sublist.Count && sublist[count].Equals(list[listIndex + count]))
                    count++;
                if (count == sublist.Count)
                    return listIndex;
            }
            return -1;
        }

        private static List<Sentences> PartialBox(Models.Drawing results, List<Sentences> lT0)
        {
            Models.Drawing reducedOCR = results;
            List<Sentences> lsO = new List<Sentences>();
            try
            {
                List<Sentences> lToBeRemoved = new List<Sentences>();
                Sentences sT = new Sentences();
                //Get longest width item(s) - Could be several items if they have same width but different words
                List<Sentences> lsT = lT0.MaxBy(s => s.Width).ToList();
                //Handle multiple identical width values prior to removing shorter widths
                if (lsT.Count > 1)
                {
                    //Content similarity test
                    Levenstein lev = new Levenstein();
                    List<Dictionary<Sentences, List<double>>> lSims = new List<Dictionary<Sentences, List<double>>>();
                    foreach (Sentences s1 in lsT)
                    {
                        Dictionary<Sentences, List<double>> dSim = new Dictionary<Sentences, List<double>>();
                        List<double> ldSim = new List<double>();
                        foreach (Sentences s2 in lsO)
                        { ldSim.Add(lev.GetSimilarity(s1.Words, s2.Words)); }
                        dSim.Add(s1, ldSim);
                        lSims.Add(dSim);
                    }

                    List<Dictionary<Sentences, double>> lSim = new List<Dictionary<Sentences, double>>();
                    foreach (var d in lSims)
                    {
                        Dictionary<Sentences, double> dSim = new Dictionary<Sentences, double>();
                        double dv = d.Sum(s => s.Value.Sum());
                        dSim.Add(d.Keys.FirstOrDefault(), d.Sum(s => s.Value.Sum()));
                        lSim.Add(dSim);
                    }
                    //Order by value and take sentence
                    lSim.OrderBy(s => s.Values);
                    sT = lSim[0].Keys.First();
                }
                else if (lsT.Count == 1)    //Only single widest value exists
                {
                    sT = lsT[0];
                }

                lsO = lT0.Except(lsT).ToList();

                if (lsO.Count(s => s.Equals(sT)) > 0)
                { lsO.Remove(sT); }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lsO;
        }

    }
}
