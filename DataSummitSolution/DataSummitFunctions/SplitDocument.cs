using DataSummitFunctions.Models;
using DataSummitFunctions.Models.Paper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json;

using SelectPdf;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions
{
    public static class SplitDocument
    {
        [FunctionName("SplitDocument")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                                            HttpRequestMessage req, TraceWriter log)// Microsoft.Extensions.Logging.ILogger log)
        {
            ImageUpload imgUp = new ImageUpload();
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                imgUp = (ImageUpload)data;
            }
            catch (Exception ae)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                { Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json") };
            }
            try
            {

                if (imgUp.Tasks == null) imgUp.Tasks = new List<Tasks>();
                if (imgUp.Layers == null) imgUp.Layers = new List<string>();

                if (imgUp.CompanyId <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: CompanyId is less than zero.");
                if (imgUp.ProjectId <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: ProjectId is less than zero.");
                if (imgUp.DrawingId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: DrawingId is less than zero.");
                //if (imgUp.Company == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Company is blank.");
                //if (imgUp.Project == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Project is blank.");
                if (imgUp.FileName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Name is ,less than zero.");
                //if (imgUp.WidthOriginal <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: WidthOriginal is less than zero.");
                //if (imgUp.HeightOriginal <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: HeightOriginal is less than zero.");
                //if (imgUp.Width <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Width is less than zero.");
                //if (imgUp.Height <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Height is less than zero.");
                //if (imgUp.X <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: X is less than zero.");
                //if (imgUp.Y <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Y is less than zero.");
                //if (imgUp.Type == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Type is blank.");
                if (imgUp.File.Length == 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: PDF/Image is empty.");

                List<ImageUpload> imgOut = new List<ImageUpload>();

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.BlobStorageName +
                                           ";AccountKey=" + imgUp.BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Blob connection";
                if (account != null) { log.Info(strError + ": failed"); }
                else { log.Info(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.Info(strError + ": failed"); }
                else { log.Info(strError + " = " + blobClient.ToString() + ": success"); }

                if (imgUp.Tasks.Count == 0)
                { imgUp.Tasks.Add(new Tasks("Connected to storage account", DateTime.Now)); }
                else
                { imgUp.Tasks.Add(new Tasks("Connected to storage account", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp)); }

                //create a pdf document.
                MemoryStream ms = new MemoryStream();
                ms = new MemoryStream(imgUp.File);
                PdfDocument pdfDoc = new PdfDocument(ms);
                Spire.Pdf.PdfDocument spirePDF = new Spire.Pdf.PdfDocument(imgUp.File);

                Models.Paper.Sizes.Load();

                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.Info(strError + ": failed"); }
                else { log.Info(strError + " = " + blobClient.ToString() + ": success"); }

                for (int i = 0; i < pdfDoc.Pages.Count; i++)
                {
                    String ContainerName = Guid.NewGuid().ToString();
                    log.Info("Container Name: " + ContainerName);

                    CloudBlobContainer blobContainer = blobClient.GetContainerReference(ContainerName);
                    blobContainer.Create();
                    BlobContainerPermissions bcp = new BlobContainerPermissions();
                    bcp.PublicAccess = BlobContainerPublicAccessType.Container;
                    blobContainer.SetPermissions(bcp);

                    imgUp.Tasks.Add(new Tasks("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": Container created", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                    log.Info("(Total: " + TimeSpan.FromTicks(imgUp.Tasks.Sum(t => t.Duration.Ticks)).ToString(@"mm\:ss\.f") +
                                       " Delta: " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString(@"mm\:ss\.f") + ") " +
                                       imgUp.Tasks[imgUp.Tasks.Count - 1].Name.ToString());

                    List<Task> lTasks = new List<Task>();
                    List<string> lLayers = new List<string>();
                    PdfDocument pdfSingle = new PdfDocument();
                    Spire.Pdf.PdfDocument spdfSingle = new Spire.Pdf.PdfDocument();
                    Spire.Pdf.PdfPageBase sPage;
                    CloudBlockBlob blockBlobJPG = null;
                    Models.Paper.Type paperType = new Models.Paper.Type();

                    lTasks.Add(Task.Run(() =>
                    {
                        //Ensure that each page is orientated correctly, which doesn't show in viewers
                        bool HasRotationIssue = false;
                        if (pdfDoc.Pages[i].Rotation == PdfPageRotation.Rotate_90 || pdfDoc.Pages[i].Rotation == PdfPageRotation.Rotate_270)
                        { HasRotationIssue = true; }

                        if (HasRotationIssue == false)
                        {
                            if (pdfDoc.Pages[i].PageSize.Width < pdfDoc.Pages[i].PageSize.Height)
                            {
                                pdfSingle.AddPage(pdfDoc.Pages[i]);
                                pdfSingle.Pages[0].Orientation = PdfPageOrientation.Portrait;
                                //May need to rotate Spire single PDF to Potrait
                                sPage = spdfSingle.Pages.Add(spirePDF.Pages[i].Size, new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                            else
                            {
                                pdfSingle.AddPage(pdfDoc.Pages[i]);
                                pdfSingle.Pages[0].Orientation = PdfPageOrientation.Landscape;
                                //May need to rotate Spire single PDF to Landscape
                                sPage = spdfSingle.Pages.Add(spirePDF.Pages[i].Size, new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                        }
                        else
                        {
                            if (pdfDoc.Pages[i].PageSize.Width < pdfDoc.Pages[i].PageSize.Height)
                            {
                                pdfSingle.AddPage(pdfDoc.Pages[i]);
                                pdfSingle.Pages[0].Orientation = PdfPageOrientation.Portrait;
                                //May need to rotate Spire single PDF to Landscape
                                sPage = spdfSingle.Pages.Add(new SizeF(spirePDF.Pages[i].Size.Height, spirePDF.Pages[i].Size.Width),
                                                                 new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                            else
                            {
                                pdfSingle.AddPage(pdfDoc.Pages[i]);
                                pdfSingle.Pages[0].Orientation = PdfPageOrientation.Landscape;
                                //May need to rotate Spire single PDF to Potrait
                                sPage = spdfSingle.Pages.Add(spirePDF.Pages[i].Size, new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                        }

                        PaperKind pkCur = Models.Paper.Sizes.Match(pdfSingle.Pages[0].PageSize.Width, pdfSingle.Pages[0].PageSize.Height);
                        paperType = Models.Paper.Sizes.All.FirstOrDefault(p => p.Kind == pkCur);
                        //Create image block blob
                        blockBlobJPG = blobContainer.GetBlockBlobReference("Original.jpg");

                        //Spire PDF image conversion
                        Image imgSpire;
                        try
                        {
                            imgSpire = spdfSingle.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap, 200, 200);
                            MemoryStream msSpireJPG = new MemoryStream();
                            imgSpire.Save(msSpireJPG, ImageFormat.Jpeg);
                            msSpireJPG.Seek(0, SeekOrigin.Begin);
                            blockBlobJPG.UploadFromStream(msSpireJPG);
                        }
                        catch (Exception ae1)
                        {
                            string strErr1 = ae1.ToString();
                            //SelectPDF image conversion
                            byte[] bDat = pdfSingle.Save();

                            //Convert PDF page to image
                            PdfRasterizer rasterizer = new PdfRasterizer();
                            rasterizer.Load(bDat);

                            // set the properties
                            rasterizer.StartPageNumber = 1;
                            rasterizer.EndPageNumber = 1;

                            // other properties that can be set
                            rasterizer.Resolution = 200;    //Cannot go above 200, otherwise an error is thrown
                            rasterizer.ColorSpace = PdfRasterizerColorSpace.RGB;

                            byte[] imgBytes = rasterizer.ConvertToTiff();
                            //List<Image> lImages = rasterizer.ConvertToImages().ToList();

                            imgUp.Tasks.Add(new Tasks("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": pdf converted to jpg'", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                            log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);

                            blockBlobJPG.UploadFromByteArray(imgBytes, 0, imgBytes.Length);
                        }



                        imgUp.Tasks.Add(new Tasks("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": uploaded to blob as 'Original.jpg'", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                        log.Info("(Total: " + TimeSpan.FromTicks(imgUp.Tasks.Sum(t => t.Duration.Ticks)).ToString(@"mm\:ss\.f") +
                                       " Delta: " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString(@"mm\:ss\.f") + ") " +
                                       imgUp.Tasks[imgUp.Tasks.Count - 1].Name);
                    }));

                    lTasks.Add(Task.Run(() =>
                    {
                        //via MemoryStream from SpirePDF (use Spire maintain layers)
                        MemoryStream msSpirePDF = new MemoryStream();
                        spdfSingle.SaveToStream(msSpirePDF, Spire.Pdf.FileFormat.PDF);
                        msSpirePDF.Seek(0, SeekOrigin.Begin);
                        CloudBlockBlob blockBlobPDF = blobContainer.GetBlockBlobReference("Original.pdf");

                        //Export PDF blockBlob data
                        blockBlobPDF.Metadata.Add("CompanyId", imgUp.CompanyId.ToString());
                        //blockBlobPDF.Metadata.Add("Company", imgUp.Company.ToString());
                        blockBlobPDF.Metadata.Add("ProjectId", imgUp.ProjectId.ToString());
                        //blockBlobPDF.Metadata.Add("Project", imgUp.Project.ToString());
                        blockBlobPDF.Metadata.Add("FileName", imgUp.FileName.ToString());

                        blockBlobPDF.UploadFromStream(msSpirePDF);

                        //Populates layer names to a list from each document, which will allow unsupervised learning
                        if (spdfSingle.Layers.Count > 0)
                        {
                            for (int l = 0; l < spdfSingle.Layers.Count; l++)
                            {
                                lLayers.Add(spirePDF.Layers[l].Name);
                            }
                        }
                        lLayers.Distinct().OrderBy(l => l);

                        imgUp.Tasks.Add(new Tasks("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": uploaded to blob as 'Original.pdf'", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                        log.Info("(Total: " + TimeSpan.FromTicks(imgUp.Tasks.Sum(t => t.Duration.Ticks)).ToString(@"mm\:ss\.f") +
                                           " Delta: " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString(@"mm\:ss\.f") + ") " +
                                           imgUp.Tasks[imgUp.Tasks.Count - 1].Name);
                    }));

                    //Ensure that all processes are finished
                    Task.WaitAll(lTasks.ToArray());
                    imgUp.Tasks.OrderBy(e => e.TimeStamp);

                    ImageUpload iu = new ImageUpload
                    {
                        CompanyId = imgUp.CompanyId,
                        //Company = imgUp.Company,
                        ProjectId = imgUp.ProjectId,
                        //Project = imgUp.Project,
                        ContainerName = Guid.NewGuid().ToString(),
                        WidthOriginal = 0,
                        HeightOriginal = 0,
                        Width = 0,
                        Height = 0,
                        X = 0,
                        Y = 0,
                        File = null,
                        FileName = imgUp.FileName,
                        Tasks = imgUp.Tasks.ToList(),
                        Type = "PDF",
                        Layers = lLayers.ToList(),
                        PaperSize = paperType,
                        UserId = imgUp.UserId
                    };

                    if (blockBlobJPG != null)
                    {
                        iu.BlobStorageName = blockBlobJPG.Name;
                        iu.BlobStorageKey = blockBlobJPG.Uri.ToString();
                        iu.BlobURL = blockBlobJPG.StorageUri.ToString();
                    }
                    imgOut.Add(iu);
                }

                string jsonToReturn = JsonConvert.SerializeObject(imgOut);

                return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json") };
            }
            catch (Exception ae)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                { Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json") };
            }
        }
    }
}
