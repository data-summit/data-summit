using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Models.Amazon;
using DataSummitFunctions.Models.Azure;
using DataSummitFunctions.Models.Google.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;

namespace DataSummitFunctions.Methods
{
    public class OCRSources
    {
        public List<Sentences> FromAmazon(List<Amazon.Rekognition.Model.TextDetection> lDetections, DataSummitFunctions.Models.ImageGrid im)
        {
            List<Sentences> Sentences = new List<Sentences>();
            try
            {
                foreach (Amazon.Rekognition.Model.TextDetection t in lDetections)
                {
                    Sentences s = new Sentences
                    {
                        //FileName = amRes.FileName
                    };
                    //ImageGrid im = Drawings.First(i => i.Name == s.FileName);
                    //ImageGrid im = Drawings.First(i => i.Path.Substring(i.Path.LastIndexOf("\\") + 1,
                    //                                        i.Path.Length - i.Path.LastIndexOf("\\") - 1) == s.FileName);
                    Models.Amazon.BoundingBox bb = new Models.Amazon.BoundingBox(
                        t.Geometry.BoundingBox.Height, t.Geometry.BoundingBox.Left,
                        t.Geometry.BoundingBox.Top, t.Geometry.BoundingBox.Width);

                    //s.Rectangle = new Models.Consolidated.Rectangle();
                    s.Left = (int)Math.Round(bb.Left * im.Width);
                    s.Top = (int)Math.Round(bb.Top * im.Height);
                    s.Width = (int)Math.Round(bb.Width * im.Width);
                    s.Height = (int)Math.Round(bb.Height * im.Height);

                    s.Words = t.DetectedText;
                    s.Vendor = "Amazon";

                    Sentences.Add(s);
                }

            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Sentences;
        }

        //public void FromAzure(Azure.AzureOCR azRes, int bordersize = 0)
        public List<Sentences> FromAzure(AzureOCR azRes, int bordersize = 0)
        {
            List<Sentences> Sentences = new List<Sentences>();
            try
            {
                foreach (Region r in azRes.Regions)
                {
                    foreach (Line l in r.Lines)
                    {
                        String a = String.Join(" ", l.Words.Select(y => y.Text));
                        if (a != "")
                        { Sentences.Add(new Sentences(a, new Models.Consolidated.Rectangle(l.RectString), "Azure", true)); }
                    }
                    //String a = String.Join(" ", r.Lines.SelectMany(l => l.Words.Select(y => y.Text)));
                    //if (a != "") Sentences.Add(new Sentence(a,
                    //                            new Rectangle(r.Rectangle.Left - bordersize, r.Rectangle.Top - bordersize,
                    //                            r.Rectangle.Width, r.Rectangle.Height), "Azure"));
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Sentences;
        }

        public List<Sentences> FromGoogle(Cloud gRes)
        {
            List<Sentences> Sentences = new List<Sentences>();
            try
            {
                foreach (Responses g in gRes.responses)
                {
                    ////Text Annotation
                    //for (int i = 1; i < g.textAnnotations.Count; i++)
                    //{
                    //    Google.Response.TextAnnotation r = g.textAnnotations[i];
                    //    Sentence s = new Sentence();
                    //    s.Words = r.description.Trim();
                    //    List<Google.Response.Vertex> lAnno = r.boundingPoly.vertices.ToList();
                    //    s.FileName = FileName;

                    //    int XMax = lAnno.MaxBy(v => v.x).First().x;
                    //    int XMin = lAnno.MinBy(v => v.x).First().x;
                    //    int YMax = lAnno.MaxBy(v => v.y).First().y;
                    //    int YMin = lAnno.MinBy(v => v.y).First().y;
                    //    s.Rectangle = new Rectangle(XMin, YMin, XMax - XMin, YMax - YMin);
                    //    s.Vendor = "Google";
                    //    Sentences.Add(s);
                    //}
                    //Full Text Annotation
                    foreach (Models.Google.Response.Page pag in g.fullTextAnnotation.pages)
                    {
                        foreach (Models.Google.Response.Block b in pag.blocks)
                        {
                            foreach (Models.Google.Response.Paragraph p in b.paragraphs)
                            {
                                List<Sentences> lSen = DivideBlockIntoLines(p, "");
                                foreach (Sentences ss in lSen)
                                { Sentences.Add(ss); }

                                //Sentence s = new Sentence();

                                //List<Google.Response.Vertex> lAnno = p.boundingBox.vertices.ToList();
                                //s.FileName = FileName;

                                //int XMax = lAnno.Max(v => v.x);
                                //int XMin = lAnno.Min(v => v.x);
                                //int YMax = lAnno.Max(v => v.y);
                                //int YMin = lAnno.Min(v => v.y);
                                //s.Rectangle = new Rectangle(XMin, YMin, XMax - XMin, YMax - YMin);

                                //s.Words = String.Join(" ", p.words.Select(se => String.Join("", se.symbols.SelectMany(te => te.text).ToList())).ToList());
                                //s.Vendor = "Google";
                                //Sentences.Add(s);
                            }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Sentences;
        }

        public List<Sentences> FromGoogle(List<Responses> lres, string filename)
        {
            List<Sentences> Sentences = new List<Sentences>();
            try
            {
                foreach (Responses g in lres)
                {
                    //Full Text Annotation
                    foreach (Models.Google.Response.Page pag in g.fullTextAnnotation.pages)
                    {
                        foreach (Models.Google.Response.Block b in pag.blocks)
                        {
                            foreach (Models.Google.Response.Paragraph p in b.paragraphs)
                            {
                                List<Sentences> lSen = DivideBlockIntoLines(p, filename);
                                foreach (Sentences ss in lSen)
                                { Sentences.Add(ss); }
                            }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return Sentences;
        }

        private List<Sentences> DivideBlockIntoLines(Models.Google.Response.Paragraph p, string filename, short Tolerance = 4)
        {
            List<Sentences> lRes = new List<Sentences>();
            List<List<string>> lLines = new List<List<string>>();
            try
            {
                List<string> sent = new List<string>();
                int senRight = int.MinValue; int senLeft = int.MinValue;
                int senBottom = int.MinValue; int senTop = int.MinValue;
                foreach (Models.Google.Response.Word w in p.words)
                {
                    //Word geometry
                    int Right = w.boundingBox.vertices.Max(v => v.x);
                    int Left = w.boundingBox.vertices.Min(v => v.x);
                    int Bottom = w.boundingBox.vertices.Max(v => v.y);
                    int Top = w.boundingBox.vertices.Min(v => v.y);

                    string wor = String.Join("", w.symbols.Select(te => te.text).ToList());
                    if (sent.Count == 0)
                    {
                        sent.Add(wor);
                        senRight = Right;
                        senLeft = Left;
                        senBottom = Bottom;
                        senTop = Top;
                    }
                    else if (Bottom < (senBottom + Tolerance) && Top > (senTop - Tolerance))
                    {
                        sent.Add(wor);
                        senRight = Right;
                    }
                    else
                    {
                        //Create previous line
                        Sentences ss = new Sentences
                        {
                            Words = string.Join(" ", sent.ToList()),
                            Left = senLeft,
                            Top = senTop,
                            Width = senRight - senLeft,
                            Height = senBottom - senTop,
                            Vendor = "Google"
                        };
                        //ss.FileName = filename;
                        lRes.Add(ss);
                        //Apply current line information
                        senLeft = Left;
                        senTop = Top;
                        senRight = Right;
                        senBottom = Bottom;
                        sent.Clear();
                        sent.Add(wor);
                    }
                }
                Sentences ssl = new Sentences
                {
                    Words = string.Join(" ", sent.ToList()),
                    Left = senLeft,
                    Top = senTop,
                    Width = senRight - senLeft,
                    Height = senBottom - senTop,
                    Vendor = "Google",
                    SlendernessRatio = (senRight - senLeft) / (senBottom - senTop)
                };
                //ssl.FileName = filename;
                lRes.Add(ssl);

            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lRes;
        }
    }
}
