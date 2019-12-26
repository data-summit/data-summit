using Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Google.Response;

namespace TemplateMapper.OCR.Consolidated
{
    public class ConsolidatedOCR
    {
        public string FileName = String.Empty;
        public List<Sentence> Sentences = new List<Sentence>();

        public ConsolidatedOCR()
        { }

        public void FromAmazon(Amazon.AmazonOCR amRes, List<ImageGrid> Drawings)
        {
            try
            {
                foreach(Amazon.TextDetection t in amRes.Results.TextDetections)
                {
                    Sentence s = new Sentence();
                    s.FileName = amRes.FileName;
                    ImageGrid im = Drawings.First(i => i.BlobUrl.Substring(i.BlobUrl.LastIndexOf("\\") + 1,
                                                            i.BlobUrl.Length - i.BlobUrl.LastIndexOf("\\") - 1) == s.FileName);
                    Amazon.BoundingBox bb = t.Geometry.BoundingBox;
                    List<double> RectAmazon = new List<double>();
                    RectAmazon.Add(bb.Left);
                    RectAmazon.Add(bb.Top);
                    RectAmazon.Add(bb.Width);
                    RectAmazon.Add(bb.Height);

                    s.Rectangle = new TemplateMapper.OCR.Consolidated.Rectangle();
                    s.Rectangle.Left = (int)Math.Round(RectAmazon[0] * im.Width);
                    s.Rectangle.Top = (int)Math.Round(RectAmazon[1] * im.Height);
                    s.Rectangle.Width = (int)Math.Round(RectAmazon[2] * im.Width);
                    s.Rectangle.Height = (int)Math.Round(RectAmazon[3] * im.Height);

                    s.SlendernessRatio = s.Rectangle.Width / s.Rectangle.Height;

                    s.Words = t.DetectedText;
                    s.Vendor = "Amazon";

                    Sentences.Add(s);
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        public void FromAzure(Azure.AzureOCR azRes, int bordersize = 0)
        {
            try
            {
                foreach (Azure.Region r in azRes.Regions)
                {
                    foreach (Azure.Line l in r.Lines)
                    {
                        String a = String.Join(" ", l.Words.Select(y => y.Text));
                        if (a != "") Sentences.Add(new Sentence(a,
                                                new Rectangle(l.Rectangle.Left - bordersize, l.Rectangle.Top - bordersize,
                                                l.Rectangle.Width, l.Rectangle.Height), "Azure", FileName));
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        public void FromGoogle(OCR.Google.Response.Cloud gRes)
        {
            try
            {
                foreach (Google.Response.Responses g in gRes.responses)
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
                    foreach (OCR.Google.Response.Page pag in g.fullTextAnnotation.pages)
                    {
                        foreach (OCR.Google.Response.Block b in pag.blocks)
                        {
                            foreach (OCR.Google.Response.Paragraph p in b.paragraphs)
                            {
                                List<Sentence> lSen = DivideBlockIntoLines(p, "");
                                foreach(Sentence ss in lSen)
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
                String strError = ae.ToString();
            }
        }

        public void FromGoogle(List<Responses> lres, string filename)
        {
            try
            {
                foreach (Responses g in lres)
                {
                    //Full Text Annotation
                    foreach (OCR.Google.Response.Page pag in g.fullTextAnnotation.pages)
                    {
                        foreach (OCR.Google.Response.Block b in pag.blocks)
                        {
                            foreach (OCR.Google.Response.Paragraph p in b.paragraphs)
                            {
                                List<Sentence> lSen = DivideBlockIntoLines(p, filename);
                                foreach (Sentence ss in lSen)
                                { Sentences.Add(ss); }
                            }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private List<Sentence> DivideBlockIntoLines(OCR.Google.Response.Paragraph p, string filename, short Tolerance = 4)
        {
            List<Sentence> lRes = new List<Sentence>();
            List<List<string>> lLines = new List<List<string>>();
            try
            {
                List<string> sent = new List<string>();
                int senRight = int.MinValue; int senLeft = int.MinValue;
                int senBottom = int.MinValue; int senTop = int.MinValue;
                foreach (Google.Response.Word w in p.words)
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
                        Sentence ss = new Sentence();
                        ss.Words = string.Join(" ", sent.ToList());
                        ss.Rectangle = new Rectangle(senLeft, senTop, senRight - senLeft, senBottom - senTop);
                        ss.Vendor = "Google";
                        ss.FileName = filename;
                        
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
                Sentence ssl = new Sentence();
                ssl.Words = string.Join(" ", sent.ToList());
                ssl.Rectangle = new Rectangle(senLeft, senTop, senRight - senLeft, senBottom - senTop);
                ssl.Vendor = "Google";
                ssl.FileName = filename;
                ssl.SlendernessRatio = (senRight - senLeft) / (senBottom - senTop);
                lRes.Add(ssl);

            }
            catch (Exception ae)
            { String strError = ae.ToString(); }
            return lRes;
        }
    }
}
