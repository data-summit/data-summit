using Models;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TemplateMapper.OCR.Consolidated;
using TemplateMapper.OCR.PostProcessing;

namespace TemplateMapper.Forms
{
    public partial class ImageSnippet : Form
    {
        private List<String> Files = new List<String>();
        private List<ImageGrid> Drawings = new List<ImageGrid>();
        public ImageSnippet()
        {
            InitializeComponent();
        }

        private void ImageSnippet_Load(object sender, EventArgs e)
        {

            PictureBox pbImage = new PictureBox();
            try
            {
                //Images from local disk
                GetImages();
                List<ImageGrid> lDrawLocal = Drawings;
                ConsolidatedOCR cOCR = new ConsolidatedOCR();

                ////Azure Computer Vision Text Recognition object
                //TemplateMapper.OCR.Azure.AzureOCR azocr = new TemplateMapper.OCR.Azure.AzureOCR();
                //Task t0 = azocr.Run();  //v2.0 ARM method
                //Task t1 = azocr.Run2();  //v2.0 REST method

                //Google Computer Vision Text Recognition object
                //TemplateMapper.OCR.Google.GoogleOCR gOCR = new TemplateMapper.OCR.Google.GoogleOCR();
                //cOCR = gOCR.Run(lDrawLocal);
                //TemplateMapper.OCR.Consolidated.Write.ToJSON(cOCR, @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Consolidated OCR Results (Google only).json");

                //Amazon Computer Vision Text Recognition object
                //TemplateMapper.OCR.Amazon.AmazonOCR gOCR = new TemplateMapper.OCR.Amazon.AmazonOCR();
                //cOCR = gOCR.Run(lDrawLocal);
                //TemplateMapper.OCR.Consolidated.Write.ToJSON(cOCR, @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Consolidated OCR Results (Amazon only).json");

                //pbImage.Dock = DockStyle.Fill;
                pbImage.SizeMode = PictureBoxSizeMode.AutoSize;
                pnSnippet.AutoScroll = true;

                TemplateMapper.OCR.Consolidated.FromFile coC = new FromFile();
                cOCR = coC.Read(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Consolidated OCR Results (Google only).json");

                //TemplateMapper.OCR.Google.FromFile goC = new TemplateMapper.OCR.Google.FromFile();
                //cOCR.Sentences.AddRange(goC.Path(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Google OCR Results (separated).json").Sentences);
                //TemplateMapper.OCR.Azure.FromFile azC = new TemplateMapper.OCR.Azure.FromFile();
                //cOCR.Sentences.AddRange(azC.Path(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Microsoft OCR Results (separated).json").Sentences);
                //TemplateMapper.OCR.Amazon.FromFile amC = new TemplateMapper.OCR.Amazon.FromFile();
                //cOCR.Sentences.AddRange(amC.Path(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Amazon OCR Results (separated).json", Drawings).Sentences);

                //String sFileName = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\Consolidated OCR Results.json";
                //File.WriteAllText(sFileName, JsonConvert.SerializeObject(cOCR));

                //ImageUpload imgG = JSONTests.Parse.ImageUploadJson(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Split Images Data, Structure & Azure.json");
                //foreach (ImageGrid ig in imgG.SplitImages)
                //{ cOCR.Sentences.AddRange(ig.Sentences); }

                //cOCR = CorrectWordLocations(cOCR);
                //ConsolidatedOCR rOCR = TemplateMapper.OCR.PostProcessing.MultipleVendors.Clean(cOCR);
                //ConsolidatedOCR rOCR = new ConsolidatedOCR();
                //rOCR.Sentences = JSONTests.Parse.SentencesJson(@"C:\Users\TomJames_zyl8law\Downloads\All OCR Results (Post 'self' processing).json");

                pnSnippet.Controls.Add(pbImage);
                //pbImage.Image = OverlayOCR(cOCR, GetImages());
                Bitmap img = (Bitmap)Desktop.Images.FromDisk(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\BG (New)\M 101_Plantroom Schematic, Heating and HWS.jpg");
                //Bitmap img = DrawingsToBitmap();
                //Bitmap img = (Bitmap)Azure.Blob.GetImage("18fc6977-38a4-4d9f-930d-cfb706fa391f");
                //rOCR.Sentences = Azure.Blob.GetSentences("18fc6977-38a4-4d9f-930d-cfb706fa391f");
                pbImage.Image = OverlayOCR(cOCR, img);
                //pbImage.Image = OverlayOCRComparison(cOCR, rOCR, img);
                //pbImage.Image = OverlayOCRUsedUnused(rOCR, img);

                //Self self = new Self();
                //rOCR.Sentences = Helper.Sentence.UpdateRatio(rOCR.Sentences);
                //rOCR.Sentences = self.Clean(rOCR.Sentences);
                //rOCR.Sentences = rOCR.Sentences.Where(s => s.IsUsed == true).ToList();
                //pbImage.Image = OverlayOCR(rOCR, img);

                ////Save result image files
                //String sFileName = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test\";
                //Boolean bAm = false; Boolean bAz = false; Boolean bGo = false;
                //if (cOCR.Sentences.Count(s => s.Vendor == "Amazon") > 0)
                //{ bAm = true; }
                //if (cOCR.Sentences.Count(s => s.Vendor == "Azure") > 0)
                //{ bAz = true; }
                //if (cOCR.Sentences.Count(s => s.Vendor == "Google") > 0)
                //{ bGo = true; }

                //if (bAm == true || bAz == true || bGo == true)
                //{
                //    if (bAm == true && bAz == true && bGo == true)
                //    { sFileName = sFileName + "Amazon, Azure & Google"; }
                //    else if (bAm == true && bAz == true && bGo == false)
                //    { sFileName = sFileName + "Amazon & Azure"; }
                //    else if (bAm == true && bAz == false && bGo == true)
                //    { sFileName = sFileName + "Amazon & Google"; }
                //    else if (bAm == false && bAz == true && bGo == true)
                //    { sFileName = sFileName + "Azure & Google"; }
                //    else if (bAm == true && bAz == false && bGo == false)
                //    { sFileName = sFileName + "Amazon"; }
                //    else if (bAm == false && bAz == true && bGo == false)
                //    { sFileName = sFileName + "Azure"; }
                //    else if (bAm == false && bAz == false && bGo == true)
                //    { sFileName = sFileName + "Google"; }

                //    sFileName = sFileName + ".jpg";
                //    if (File.Exists(sFileName)) File.Delete(sFileName);
                //    pbImage.Image.Save(sFileName);
                //}
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void GetImages()
        {
            try
            {
                String strFolPath = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Image Splitter Test";
                Desktop.CSV.FromDisk(strFolPath);
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            //return null;
        }
        //private Bitmap DrawingsToBitmap()
        //{
        //    Bitmap bitCom = null;
        //    try
        //    {
        //        int width = 0; int height = 0;
        //        List<ImageGrid> lRegDrawings = Drawings.Where(d => d.Type == ImageGrid.ImageType.Normal).ToList();
        //        //Add drawings to bitmap
        //        width = lRegDrawings.Where(d => d.HeightStart == 0).Sum(e => e.Width);
        //        height = lRegDrawings.Where(d => d.WidthStart == 0).Sum(e => e.Height);
        //        bitCom = new Bitmap(width, height); //, PixelFormat.Undefined);
        //        Graphics grCom = Graphics.FromImage(bitCom);
                
        //        foreach (ImageGrid im in lRegDrawings)
        //        {
        //            grCom.DrawImage(im.Image, im.WidthStart, im.HeightStart, im.Width, im.Height);
        //        }
        //    }
        //    catch (Exception ae)
        //    {
        //        String strError = ae.ToString();
        //    }
        //    return bitCom;
        //}
        private ConsolidatedOCR CorrectWordLocations(ConsolidatedOCR cOCR)
        {
            ConsolidatedOCR rOCR = new ConsolidatedOCR();
            try
            {

                foreach (Sentence s in cOCR.Sentences)
                {
                    if (Drawings.Count(n => n.Name == s.FileName) > 0)
                    {
                        ImageGrid im = Drawings.First(i => i.Name == s.FileName);
                        s.Rectangle = new TemplateMapper.OCR.Consolidated.Rectangle(
                                            im.WidthStart + s.Rectangle.Left,
                                            im.HeightStart + s.Rectangle.Top, 
                                            s.Rectangle.Width, s.Rectangle.Height);
                        rOCR.Sentences.Add(s);
                    }
                    else
                    {
                        string sna = s.FileName;
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return rOCR;
        }
        private Bitmap OverlayOCR(ConsolidatedOCR cOCR, Bitmap cImg, bool AddIndexNumber = true)
        {
            try
            {
                //List<String> lAllFiles = cOCR.Sentences.Select(s => s.FileName).Distinct().ToList();
                //List<String> lfFiles = lAllFiles.Where(s => s.Contains("F")).ToList();
                Graphics g = Graphics.FromImage(cImg);

                int iTransparency = 80;
                Pen pnAmazon = new Pen(Color.FromArgb(iTransparency, 255, 215, 0), 2);
                Pen pnAzure = new Pen(Color.FromArgb(iTransparency, 0, 0, 255), 2);
                Pen pnGoogle = new Pen(Color.FromArgb(iTransparency, 255, 0, 0), 2);

                foreach (Sentence s in cOCR.Sentences)
                {
                    if (s.Vendor == "Amazon")
                    {
                        g.DrawRectangle(pnAmazon, s.Rectangle.Left,
                            s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height);
                    }
                    if (s.Vendor == "Azure")
                    {
                        g.DrawRectangle(pnAzure, s.Rectangle.Left,
                            s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height);
                        if (AddIndexNumber == true)
                        {
                            var stringSize = g.MeasureString(cOCR.Sentences.IndexOf(s).ToString(), this.Font);
                            float scale = (float)((cImg.Width / stringSize.Width) + 50);
                            if (scale < 1)
                            { g.ScaleTransform(scale, scale); }
                            g.DrawString(cOCR.Sentences.IndexOf(s).ToString(), this.Font, Brushes.Blue,
                                new PointF(s.Rectangle.Left, s.Rectangle.Top + s.Rectangle.Height + 5));
                        }
                    }
                    if (s.Vendor == "Google")
                    {
                        g.DrawRectangle(pnGoogle, s.Rectangle.Left,
                            s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height);

                        if (AddIndexNumber == true)
                        {
                            var stringSize = g.MeasureString(cOCR.Sentences.IndexOf(s).ToString(), this.Font);
                            float scale = (float)((cImg.Width / stringSize.Width) + 50);
                            if (scale < 1)
                            { g.ScaleTransform(scale, scale); }
                            g.DrawString(cOCR.Sentences.IndexOf(s).ToString(), this.Font, Brushes.Red,
                                new PointF(s.Rectangle.Left, s.Rectangle.Top + s.Rectangle.Height + 5));
                        }
                    }

                    
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cImg;
        }
        private Bitmap OverlayOCRUsedUnused(ConsolidatedOCR cOCR, Bitmap cImg, bool AddIndexNumber = true)
        {
            try
            {
                Graphics g = Graphics.FromImage(cImg);

                int iTransparency = 80;
                Pen pnUsed = new Pen(Color.FromArgb(iTransparency, 255, 215, 0), 2);
                Pen pnUnused = new Pen(Color.FromArgb(iTransparency, 0, 0, 255), 2);

                foreach (Sentence s in cOCR.Sentences)
                {
                    if (s.IsUsed == true)
                    {
                        g.DrawRectangle(pnUsed, s.Rectangle.Left,
                            s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height);
                        if (AddIndexNumber == true)
                        {
                            var stringSize = g.MeasureString(cOCR.Sentences.IndexOf(s).ToString(), this.Font);
                            float scale = (float)((cImg.Width / stringSize.Width) + 50);
                            if (scale < 1)
                            { g.ScaleTransform(scale, scale); }
                            g.DrawString(cOCR.Sentences.IndexOf(s).ToString(), this.Font, Brushes.Blue,
                                new PointF(s.Rectangle.Left, s.Rectangle.Top + s.Rectangle.Height + 5));
                        }
                    }
                    else
                    {
                        g.DrawRectangle(pnUnused, s.Rectangle.Left,
                            s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height);

                        if (AddIndexNumber == true)
                        {
                            var stringSize = g.MeasureString(cOCR.Sentences.IndexOf(s).ToString(), this.Font);
                            float scale = (float)((cImg.Width / stringSize.Width) + 50);
                            if (scale < 1)
                            { g.ScaleTransform(scale, scale); }
                            g.DrawString(cOCR.Sentences.IndexOf(s).ToString(), this.Font, Brushes.Red,
                                new PointF(s.Rectangle.Left, s.Rectangle.Top + s.Rectangle.Height + 5));
                        }
                    }


                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cImg;
        }
        private Bitmap OverlayOCRComparison(ConsolidatedOCR cOCR, ConsolidatedOCR rOCR, Bitmap cImg, bool AddIndexNumber = true)
        {
            try
            {
                List<String> lAllFiles = cOCR.Sentences.Select(s => s.FileName).Distinct().ToList();
                List<String> lfFiles = lAllFiles.Where(s => s.Contains("F")).ToList();
                Graphics g = Graphics.FromImage(cImg);

                int iTransparency = 80;
                Pen pnAmazon = new Pen(Color.FromArgb(iTransparency, 255, 215, 0), 2);
                Pen pnAzure = new Pen(Color.FromArgb(iTransparency, Color.Blue), 2);
                Pen pnGoogle = new Pen(Color.FromArgb(iTransparency, 255, 0, 0), 2);
                Pen pnComparison = new Pen(Color.FromArgb(iTransparency, Color.DarkRed), 2);

                foreach (Sentence s in cOCR.Sentences)
                {
                    g.DrawRectangle(pnComparison, s.Rectangle.Left - 2,
                        s.Rectangle.Top - 2, s.Rectangle.Width + 4, s.Rectangle.Height + 4);
                    if (AddIndexNumber == true)
                    {
                        var stringSize = g.MeasureString(cOCR.Sentences.IndexOf(s).ToString(), this.Font);
                        float scale = (float)((cImg.Width / stringSize.Width) + 50);
                        if (scale < 1)
                        { g.ScaleTransform(scale, scale); }
                        g.DrawString(cOCR.Sentences.IndexOf(s).ToString(), this.Font, Brushes.Pink,
                            new PointF(s.Rectangle.Left + s.Rectangle.Width - 10, s.Rectangle.Top + s.Rectangle.Height + 5));
                    }

                }

                foreach (Sentence s in rOCR.Sentences)
                {
                    g.DrawRectangle(pnAzure, s.Rectangle.Left,
                        s.Rectangle.Top, s.Rectangle.Width, s.Rectangle.Height);
                    if (AddIndexNumber == true)
                    {
                        var stringSize = g.MeasureString(cOCR.Sentences.IndexOf(s).ToString(), this.Font);
                        float scale = (float)((cImg.Width / stringSize.Width) + 50);
                        if (scale < 1)
                        { g.ScaleTransform(scale, scale); }
                        g.DrawString(cOCR.Sentences.IndexOf(s).ToString(), this.Font, Brushes.Blue,
                            new PointF(s.Rectangle.Left, s.Rectangle.Top + s.Rectangle.Height + 5));
                    }

                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cImg;
        }

        
    }
}
