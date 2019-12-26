using MoreLinq;
using SimMetricsMetricUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Consolidated;

namespace TemplateMapper.OCR
{
    public static class PostprocessingRefactorRequired
    {
        public static ConsolidatedOCR Clean(ConsolidatedOCR cOCR)
        {
            ConsolidatedOCR rOCR = new ConsolidatedOCR();
            try
            {
                List<String> lFN = cOCR.Sentences.Select(e => e.Words).OrderBy(e => e).ToList();
                rOCR = RemoveDuplicates(cOCR);
                rOCR = WordlessRectangles(rOCR);
                rOCR = Encapsulated(rOCR);
                rOCR = PartialOverlap(rOCR, 5);
                     
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        private static ConsolidatedOCR RemoveDuplicates(ConsolidatedOCR cOCR)
        {
            ConsolidatedOCR rOCR = new ConsolidatedOCR();
            try
            {
                rOCR.Sentences = cOCR.Sentences.Distinct().ToList();
                rOCR.FileName = cOCR.FileName;
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        private static ConsolidatedOCR WordlessRectangles(ConsolidatedOCR cOCR)
        {
            ConsolidatedOCR rOCR = new ConsolidatedOCR();
            try
            {
                rOCR.Sentences = cOCR.Sentences.Where(e => e.Words.ToString() != "").ToList();
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        private static ConsolidatedOCR Encapsulated(ConsolidatedOCR cOCR)
        {
            ConsolidatedOCR rOCR = cOCR;
            try
            {
                List<Sentence> lS = new List<Sentence>();
                List<Sentence> lR = new List<Sentence>();
                Levenstein lev = new Levenstein();
                EuclideanDistance eDis = new EuclideanDistance();
                foreach (Sentence s in cOCR.Sentences)
                {
                    //lS.AddRange(cOCR.Sentences
                    //                .Where(e => e.FileName != s.FileName &&
                    //                        e.Rectangle.Top <= s.Rectangle.Top &&
                    //                        e.Rectangle.Left <= s.Rectangle.Left &&
                    //                        (e.Rectangle.Top + e.Rectangle.Height) >= (s.Rectangle.Top + s.Rectangle.Height) &&
                    //                        (e.Rectangle.Left + e.Rectangle.Width) >= (s.Rectangle.Left + s.Rectangle.Width)
                    //                        ).ToList());
                    List<Sentence> ss = cOCR.Sentences
                                    .Where(e => e.Vendor != s.Vendor &&
                                            e.Rectangle.Top <= s.Rectangle.Top &&
                                            e.Rectangle.Left <= s.Rectangle.Left &&
                                            (e.Rectangle.Top + e.Rectangle.Height) >= (s.Rectangle.Top + s.Rectangle.Height) &&
                                            (e.Rectangle.Left + e.Rectangle.Width) >= (s.Rectangle.Left + s.Rectangle.Width)
                                            ).ToList();
                    if (ss.Count > 0)
                    {
                        List<String> lan = ss.Select(e => String.Concat(e.Vendor, ": ", e.Words)).ToList();
                        lan.Add(String.Concat(s.Vendor, ": ", s.Words));
                        List<String> lan2 = ss.Select(e =>e.Words).ToList();
                        lan2.Add(s.Words);
                        Double simlev = lev.GetSimilarity(lan2[0], lan2[1]);
                        Double simeDis = eDis.GetSimilarity(lan2[0], lan2[1]);
                        lR = ss.ToList();
                        lR.Add(s);
                    }
                }
                List<Sentence> lAmazon = lS.Where(e => e.Vendor == "Amazon").ToList();
                List<Sentence> lAzure = lS.Where(e => e.Vendor == "Azure").ToList();
                List<Sentence> lGoogle = lS.Where(e => e.Vendor == "Google").ToList();
                List<String> lS1 = lS.Select(e => e.Words).OrderBy(e => e).ToList();
                List<String> lS2 = lS.Select(e => e.Words).Distinct().OrderBy(e => e).ToList();
                List<Sentence> lR2 = lS.DistinctBy(e => e.Words).ToList();
                foreach (Sentence rs in lR2)
                {
                    rOCR.Sentences.Remove(rs);
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        private static ConsolidatedOCR PartialOverlap(ConsolidatedOCR cOCR, int Tolerance)
        {
            ConsolidatedOCR rOCR = cOCR;
            try
            {
                List<Sentence> lS = new List<Sentence>();
                //List<Sentence> lS = cOCR.Sentences.Where(e => e.Words.Contains(" No") == true).ToList();
                //List<Rectangle> lR = lS.Select(e => e.Rectangle).ToList();
                foreach (Sentence s in cOCR.Sentences)
                {
                    lS.AddRange(cOCR.Sentences.Where(e =>
                                    e.Vendor != s.Vendor &&
                                    (Math.Abs(e.Rectangle.Top - s.Rectangle.Top) <= Tolerance) &&
                                    (Math.Abs(e.Rectangle.Left- s.Rectangle.Left) <= Tolerance) &&
                                    (Math.Abs(e.Rectangle.Height - s.Rectangle.Height) <= Tolerance) &&
                                    (Math.Abs(e.Rectangle.Width - s.Rectangle.Width) <= Tolerance)
                                ).ToList());
                }
                List<String> lS1 = lS.Select(e => e.Words).OrderBy(e => e).ToList();
                List<String> lS2 = lS.Select(e => e.Words).Distinct().OrderBy(e => e).ToList();
                List<Sentence> lR = lS.DistinctBy(e => e.Words).ToList();
                foreach(Sentence s in lR)
                {
                    rOCR.Sentences.Remove(s);
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        private static ConsolidatedOCR MergeOverlaps(ConsolidatedOCR cOCR)
        {
            ConsolidatedOCR rOCR = new ConsolidatedOCR();
            try
            {
                
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        public static List<Similarity> SimilarityTests(ConsolidatedOCR cOCR)
        {
            List<Similarity> lS = new List<Similarity>();
            try
            {
                List<Geo> lG = new List<Geo>();
                List<String> lV = cOCR.Sentences.Select(e => e.Vendor).Distinct().ToList();
                foreach (String sVen in lV)
                {
                    List<Sentence> lv = cOCR.Sentences.Where(e => e.Vendor == sVen).ToList();
                    List<Sentence> lnv = cOCR.Sentences.Where(e => e.Vendor != sVen).ToList();
                    foreach (Sentence sen in lv)
                    {
                        Geo g = new Geo();
                        g.sentence = sen;
                        foreach (Sentence osen in lnv)
                        {
                            //Test rectangle for geometry interactions
                            if (osen.Rectangle.ToRect().Contains(sen.Rectangle.ToRect()) == true)
                            { //Tolerance to be included for very near encapsulation
                                if (sen.Rectangle.IsAlmostEqualTo(osen.Rectangle))
                                { g.Add(osen, Geo.GeoType.Match); }
                                else
                                { g.Add(osen, Geo.GeoType.Surrounded); }
                            }
                            else if (sen.Rectangle.ToRect().Contains(osen.Rectangle.ToRect()) == true)
                            {   //Tolerance to be included for very near encapsulation
                                if (sen.Rectangle.IsAlmostEqualTo(osen.Rectangle))
                                { g.Add(osen, Geo.GeoType.Match); }
                                else
                                { g.Add(osen, Geo.GeoType.Encaspulates); }
                            }
                            else if (sen.Rectangle.IsAlmostEqualTo(osen.Rectangle))
                            { g.Add(osen, Geo.GeoType.Match); }
                            else if (sen.Rectangle.ToPoints().Count(e => osen.Rectangle.ToRect().Contains(e)) > 0)
                            { g.Add(osen, Geo.GeoType.Overlap); }
                        }
                        if (g.lOthers.Count > 0) lG.Add(g);

                    }
                }

                //Check if sentences and geometry count matches
                if (lG.Count(e => e.lGeo.Count != e.lOthers.Count) > 0)
                { throw new Exception("Geometry and sentences count mismatch"); }

                List<Geo> lSingle = lG.Where(e => e.lGeo.Count == 1 && e.lOthers.Count == 1).ToList();

                //Populate similarity tests
                String strFile = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\OCR Similiarities.txt";
                if (File.Exists(strFile) == true) File.Delete(strFile);

                StreamWriter s = new StreamWriter(strFile);
                s.WriteLine("Vendor1\tVendor2\tText1\tText2\tTest\tSub Test\tValue");
                foreach(Geo g in lSingle)
                {
                    GetSims(g).ForEach(e => s.WriteLine(e));
                }
                s.Close();

                //Perform redactive test on multiple 
                List<Geo> lMulti = lG.Where(e => e.lGeo.Count > 1).ToList();
                
                


            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return lS;
        }
        private static List<String> GetSims(Geo g)
        {
            List<String> lS = new List<string>();
            try
            {
                BlockDistance bd = new BlockDistance();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.SelectMany(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.SelectMany(e => e.Words).ToList()) + "\t" +
                        "BlockDistance\tGetSimilarity\t" +
                        bd.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.SelectMany(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "BlockDistance\tGetSimilarityExplained\t" +
                //        bd.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "BlockDistance\tGetSimilarityTimingActual\t" +
                        bd.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "BlockDistance\tGetSimilarityTimingEstimated\t" +
                        bd.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "BlockDistance\tGetUnnormalisedSimilarity\t" +
                        bd.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                ChapmanLengthDeviation cld = new ChapmanLengthDeviation();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanLengthDeviation\tGetSimilarity\t" +
                        cld.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "ChapmanLengthDeviation\tGetSimilarityExplained\t" +
                //        cld.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanLengthDeviation\tGetSimilarityTimingActual\t" +
                        cld.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanLengthDeviation\tGetSimilarityTimingEstimated\t" +
                        cld.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanLengthDeviation\tGetUnnormalisedSimilarity\t" +
                        cld.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                ChapmanMeanLength cml = new ChapmanMeanLength();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanMeanLength\tGetSimilarity\t" +
                        cml.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "ChapmanMeanLength\tGetSimilarityExplained\t" +
                //        cml.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanMeanLength\tGetSimilarityTimingActual\t" +
                        cml.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanMeanLength\tGetSimilarityTimingEstimated\t" +
                        cml.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "ChapmanMeanLength\tGetUnnormalisedSimilarity\t" +
                        cml.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                CosineSimilarity cs = new CosineSimilarity();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "CosineSimilarity\tGetSimilarity\t" +
                        cs.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "CosineSimilarity\tGetSimilarityExplained\t" +
                //        cs.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "CosineSimilarity\tGetSimilarityTimingActual\t" +
                        cs.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "CosineSimilarity\tGetSimilarityTimingEstimated\t" +
                        cs.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "CosineSimilarity\tGetUnnormalisedSimilarity\t" +
                        cs.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                DiceSimilarity ds = new DiceSimilarity();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "DiceSimilarity\tGetSimilarity\t" +
                        ds.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "DiceSimilarity\tGetSimilarityExplained\t" +
                //        ds.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "DiceSimilarity\tGetSimilarityTimingActual\t" +
                        ds.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "DiceSimilarity\tGetSimilarityTimingEstimated\t" +
                        ds.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "DiceSimilarity\tGetUnnormalisedSimilarity\t" +
                        ds.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                EuclideanDistance ed = new EuclideanDistance();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "EuclideanDistance\tGetSimilarity\t" +
                        ed.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "EuclideanDistance\tGetSimilarityExplained\t" +
                //        ed.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "EuclideanDistance\tGetSimilarityTimingActual\t" +
                        ed.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "EuclideanDistance\tGetSimilarityTimingEstimated\t" +
                        ed.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "EuclideanDistance\tGetUnnormalisedSimilarity\t" +
                        ed.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                JaccardSimilarity js = new JaccardSimilarity();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaccardSimilarity\tGetSimilarity\t" +
                        js.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "JaccardSimilarity\tGetSimilarityExplained\t" +
                //        js.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaccardSimilarity\tGetSimilarityTimingActual\t" +
                        js.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaccardSimilarity\tGetSimilarityTimingEstimated\t" +
                        js.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaccardSimilarity\tGetUnnormalisedSimilarity\t" +
                        js.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                Jaro j = new Jaro();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Jaro\tGetSimilarity\t" +
                        j.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "Jaro\tGetSimilarityExplained\t" +
                //        j.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Jaro\tGetSimilarityTimingActual\t" +
                        j.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Jaro\tGetSimilarityTimingEstimated\t" +
                        j.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Jaro\tGetUnnormalisedSimilarity\t" +
                        j.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                JaroWinkler jw = new JaroWinkler();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaroWinkler\tGetSimilarity\t" +
                        jw.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "JaroWinkler\tGetSimilarityExplained\t" +
                //        jw.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaroWinkler\tGetSimilarityTimingActual\t" +
                        jw.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaroWinkler\tGetSimilarityTimingEstimated\t" +
                        jw.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "JaroWinkler\tGetUnnormalisedSimilarity\t" +
                        jw.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                Levenstein l = new Levenstein();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Levenstein\tGetSimilarity\t" +
                        l.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "Levenstein\tGetSimilarityExplained\t" +
                //        l.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Levenstein\tGetSimilarityTimingActual\t" +
                        l.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Levenstein\tGetSimilarityTimingEstimated\t" +
                        l.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "Levenstein\tGetUnnormalisedSimilarity\t" +
                        l.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                MatchingCoefficient mc = new MatchingCoefficient();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MatchingCoefficient\tGetSimilarity\t" +
                        mc.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "MatchingCoefficient\tGetSimilarityExplained\t" +
                //        mc.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MatchingCoefficient\tGetSimilarityTimingActual\t" +
                        mc.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MatchingCoefficient\tGetSimilarityTimingEstimated\t" +
                        mc.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MatchingCoefficient\tGetUnnormalisedSimilarity\t" +
                        mc.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                MongeElkan me = new MongeElkan();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MongeElkan\tGetSimilarity\t" +
                        me.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "MongeElkan\tGetSimilarityExplained\t" +
                //        me.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MongeElkan\tGetSimilarityTimingActual\t" +
                        me.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MongeElkan\tGetSimilarityTimingEstimated\t" +
                        me.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "MongeElkan\tGetUnnormalisedSimilarity\t" +
                        me.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                NeedlemanWunch nw = new NeedlemanWunch();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "NeedlemanWunch\tGetSimilarity\t" +
                        nw.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "NeedlemanWunch\tGetSimilarityExplained\t" +
                //        nw.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "NeedlemanWunch\tGetSimilarityTimingActual\t" +
                        nw.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "NeedlemanWunch\tGetSimilarityTimingEstimated\t" +
                        nw.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "NeedlemanWunch\tGetUnnormalisedSimilarity\t" +
                        nw.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                OverlapCoefficient oc = new OverlapCoefficient();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "OverlapCoefficient\tGetSimilarity\t" +
                        oc.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "OverlapCoefficient\tGetSimilarityExplained\t" +
                //        oc.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "OverlapCoefficient\tGetSimilarityTimingActual\t" +
                        oc.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "OverlapCoefficient\tGetSimilarityTimingEstimated\t" +
                        oc.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "OverlapCoefficient\tGetUnnormalisedSimilarity\t" +
                        oc.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                QGramsDistance qdg = new QGramsDistance();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "QGramsDistance\tGetSimilarity\t" +
                        qdg.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "QGramsDistance\tGetSimilarityExplained\t" +
                //        qdg.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "QGramsDistance\tGetSimilarityTimingActual\t" +
                        qdg.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "QGramsDistance\tGetSimilarityTimingEstimated\t" +
                        qdg.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "QGramsDistance\tGetUnnormalisedSimilarity\t" +
                        qdg.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                SmithWaterman sw = new SmithWaterman();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWaterman\tGetSimilarity\t" +
                        sw.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "SmithWaterman\tGetSimilarityExplained\t" +
                //        sw.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWaterman\tGetSimilarityTimingActual\t" +
                        sw.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWaterman\tGetSimilarityTimingEstimated\t" +
                        sw.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWaterman\tGetUnnormalisedSimilarity\t" +
                        sw.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                SmithWatermanGotoh swg = new SmithWatermanGotoh();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotoh\tGetSimilarity\t" +
                        swg.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "SmithWatermanGotoh\tGetSimilarityExplained\t" +
                //        swg.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotoh\tGetSimilarityTimingActual\t" +
                        swg.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotoh\tGetSimilarityTimingEstimated\t" +
                        swg.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotoh\tGetUnnormalisedSimilarity\t" +
                        swg.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));

                SmithWatermanGotohWindowedAffine swgwa = new SmithWatermanGotohWindowedAffine();
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotohWindowedAffine\tGetSimilarity\t" +
                        swgwa.GetSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                //lS.Add(g.sentence.Vendor + "\t" + String.Join("",g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                //        g.sentence.Words + "\t" + String.Join("",g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                //        "SmithWatermanGotohWindowedAffine\tGetSimilarityExplained\t" +
                //        swgwa.GetSimilarityExplained(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotohWindowedAffine\tGetSimilarityTimingActual\t" +
                        swgwa.GetSimilarityTimingActual(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotohWindowedAffine\tGetSimilarityTimingEstimated\t" +
                        swgwa.GetSimilarityTimingEstimated(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
                lS.Add(g.sentence.Vendor + "\t" + String.Join("", g.lOthers.Select(e => e.Vendor).ToList()) + "\t" +
                        g.sentence.Words + "\t" + String.Join("", g.lOthers.Select(e => e.Words).ToList()) + "\t" +
                        "SmithWatermanGotohWindowedAffine\tGetUnnormalisedSimilarity\t" +
                        swgwa.GetUnnormalisedSimilarity(g.sentence.Words, String.Join("", g.lOthers.Select(e => e.Words).ToList())));
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return lS;
        }
    }
    public class Geo
    {
        public Sentence sentence { get; set; }
        public List<Sentence> lOthers { get; set; }
        public List<GeoType> lGeo { get; set; }
        public List<Similarity> lSim { get; set; }

        public Geo()
        {
            lOthers = new List<Sentence>();
            lGeo = new List<GeoType>();
            lSim = new List<Similarity>();
        }

        public void Add(Sentence s, GeoType gt)
        {
            lOthers.Add(s);
            lGeo.Add(gt);
        }

        public enum GeoType
        {
            Overlap,
            Encaspulates,
            Surrounded,
            Match
        }
    }
    
    public class Similarity
    {
        public String Text { get; set; }
        public String Vendor { get; set; }
        public Double dBlockDistance { get; set; }
        public Double dChapmanLengthDeviation { get; set; }
        public Double dChapmanMeanLength { get; set; }
        public Double dCosineSimilarity { get; set; }
        public Double dDiceSimilarity { get; set; }
        public Double dEuclideanDistance { get; set; }
        public Double dJaccardSimilarity { get; set; }
        public Double dJaro { get; set; }
        public Double dJaroWinkler { get; set; }
        public Double dLevenstein { get; set; }
        public Double dMatchingCoefficient { get; set; }
        public Double dMongeElkan { get; set; }
        public Double dNeedlemanWunch { get; set; }
        public Double dOverlapCoefficient { get; set; }
        public Double dQGramsDistance { get; set; }
        public Double dSmithWaterman { get; set; }
        public Double dSmithWatermanGotoh { get; set; }
        public Double dSmithWatermanGotohWindowedAffine { get; set; }

        public Similarity()
        { }
    }
    //public class Similarity
    //{
    //    public Boolean bTop { get; set; }
    //    public Boolean bLeft { get; set; }
    //    public Boolean bWidth { get; set; }
    //    public Boolean bHeight { get; set; }
    //    public Double Levenstein { get; set; }

    //    public Similarity()
    //    { }

    //    public Similarity(Boolean top, Boolean left, Boolean width, Boolean height)
    //    {
    //        bTop = top;
    //        bLeft = left;
    //        bWidth = width;
    //        bHeight = height;
    //    }

    //    public Similarity(Boolean top, Boolean left, Boolean width, Boolean height, Double levenstein)
    //    {
    //        bTop = top;
    //        bLeft = left;
    //        bWidth = width;
    //        bHeight = height;
    //        Levenstein = levenstein;
    //    }
    //}
}
//BlockDistance bd = new BlockDistance();
//ChapmanLengthDeviation cld = new ChapmanLengthDeviation();
//ChapmanMeanLength cml = new ChapmanMeanLength();
//CosineSimilarity cs = new CosineSimilarity();
//DiceSimilarity ds = new DiceSimilarity();
//EuclideanDistance ed = new EuclideanDistance();
//JaccardSimilarity js = new JaccardSimilarity();
//Jaro j = new Jaro();
//JaroWinkler jw = new JaroWinkler();
//Levenstein l = new Levenstein();
//MatchingCoefficient mc = new MatchingCoefficient();
//MongeElkan me = new MongeElkan();
//NeedlemanWunch nw = new NeedlemanWunch();
//OverlapCoefficient oc = new OverlapCoefficient();
//QGramsDistance qdg = new QGramsDistance();
//SmithWaterman sw = new SmithWaterman();
//SmithWatermanGotoh swg = new SmithWatermanGotoh();
//SmithWatermanGotohWindowedAffine swgwa = new SmithWatermanGotohWindowedAffine();