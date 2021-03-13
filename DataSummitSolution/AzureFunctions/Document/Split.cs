using DataSummitModels.DB;
using DataSummitModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
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
using System.Threading.Tasks;

namespace AzureFunctions.Document
{
    public static class Split
    {
        [FunctionName("Split")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            ImageUpload imgUp = new ImageUpload();
            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                name = name ?? data?.name;
                imgUp = (ImageUpload)data;
            }
            catch (Exception ae)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ae));
            }
            try
            {
                List<FunctionTask> Tasks = new List<FunctionTask>();

                if (imgUp.Tasks == null) imgUp.Tasks = new List<DataSummitModels.DB.Task>();
                if (imgUp.Layers == null) imgUp.Layers = new List<DocumentLayer>();

                if (imgUp.CompanyId <= 0) return new BadRequestObjectResult("Illegal input: CompanyId is less than zero.");
                if (imgUp.ProjectId <= 0) return new BadRequestObjectResult("Illegal input: ProjectId is less than zero.");
                if (imgUp.DocumentId < 0) return new BadRequestObjectResult("Illegal input: DocumentId is less than zero.");
                //if (imgUp.Company == "") return new BadRequestObjectResult("Illegal input: Company is blank.");
                //if (imgUp.Project == "") return new BadRequestObjectResult("Illegal input: Project is blank.");
                if (imgUp.FileName == "") return new BadRequestObjectResult("Illegal input: Name is ,less than zero.");
                //if (imgUp.WidthOriginal <= 0) return new BadRequestObjectResult("Illegal input: WidthOriginal is less than zero.");
                //if (imgUp.HeightOriginal <= 0) return new BadRequestObjectResult("Illegal input: HeightOriginal is less than zero.");
                //if (imgUp.Width <= 0) return new BadRequestObjectResult("Illegal input: Width is less than zero.");
                //if (imgUp.Height <= 0) return new BadRequestObjectResult("Illegal input: Height is less than zero.");
                //if (imgUp.X <= 0) return new BadRequestObjectResult("Illegal input: X is less than zero.");
                //if (imgUp.Y <= 0) return new BadRequestObjectResult("Illegal input: Y is less than zero.");
                //if (imgUp.Type == "") return new BadRequestObjectResult("Illegal input: Type is blank.");
                if (imgUp.File.Length == 0) return new BadRequestObjectResult("Illegal input: PDF/Image is empty.");

                List<ImageUpload> imgOut = new List<ImageUpload>();

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.StorageAccountName +
                                           ";AccountKey=" + imgUp.StorageAccountKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Blob connection";
                if (account != null) { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                if (Tasks.Count == 0)
                { Tasks.Add(new FunctionTask("Connected to storage account", DateTime.Now)); }
                else
                { Tasks.Add(new FunctionTask("Connected to storage account", imgUp.Tasks[Tasks.Count - 1].TimeStamp)); }

                //create a pdf document.
                MemoryStream ms = new MemoryStream();
                ms = new MemoryStream(imgUp.File);
                SelectPdf.PdfDocument pdfDoc = new SelectPdf.PdfDocument(ms);
                Spire.Pdf.PdfDocument spirePDF = new Spire.Pdf.PdfDocument(imgUp.File);

                //Populates a List with various 
                //DataSummitModels.Enums.Paper.Load();

                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                for (int i = 0; i < pdfDoc.Pages.Count; i++)
                {
                    string ContainerName = Guid.NewGuid().ToString();
                    log.LogInformation("Container Name: " + ContainerName);

                    CloudBlobContainer blobContainer = blobClient.GetContainerReference(ContainerName);
                    await blobContainer.CreateAsync();
                    BlobContainerPermissions bcp = new BlobContainerPermissions();
                    bcp.PublicAccess = BlobContainerPublicAccessType.Container;
                    await blobContainer.SetPermissionsAsync(bcp);

                    Tasks.Add(new FunctionTask("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": Container created", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                    log.LogInformation("(Total: " + TimeSpan.FromTicks(Tasks.Sum(t => t.Duration.Ticks)).ToString(@"mm\:ss\.f") +
                                       " Delta: " + imgUp.Tasks[Tasks.Count - 1].Duration.ToString(@"mm\:ss\.f") + ") " +
                                       imgUp.Tasks[Tasks.Count - 1].Name.ToString());

                    List<System.Threading.Tasks.Task> lTasks = new List<System.Threading.Tasks.Task>();
                    List<string> lLayers = new List<string>();
                    SelectPdf.PdfDocument pdfSingle = new SelectPdf.PdfDocument();
                    Spire.Pdf.PdfDocument spdfSingle = new Spire.Pdf.PdfDocument();
                    Spire.Pdf.PdfPageBase sPage;
                    CloudBlockBlob blockBlobJPG = null;
                    DataSummitModels.DTO.Paper.Types paperType = new DataSummitModels.DTO.Paper.Types();

                    lTasks.Add(System.Threading.Tasks.Task.Run(async () =>
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
                                pdfSingle.Pages[0].Orientation = SelectPdf.PdfPageOrientation.Portrait;
                                //May need to rotate Spire single PDF to Potrait
                                sPage = spdfSingle.Pages.Add(spirePDF.Pages[i].Size, new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                            else
                            {
                                pdfSingle.AddPage(pdfDoc.Pages[i]);
                                pdfSingle.Pages[0].Orientation = SelectPdf.PdfPageOrientation.Landscape;
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
                                pdfSingle.Pages[0].Orientation = SelectPdf.PdfPageOrientation.Portrait;
                                //May need to rotate Spire single PDF to Landscape
                                sPage = spdfSingle.Pages.Add(new SizeF(spirePDF.Pages[i].Size.Height, spirePDF.Pages[i].Size.Width),
                                                                 new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                            else
                            {
                                pdfSingle.AddPage(pdfDoc.Pages[i]);
                                pdfSingle.Pages[0].Orientation = SelectPdf.PdfPageOrientation.Landscape;
                                //May need to rotate Spire single PDF to Potrait
                                sPage = spdfSingle.Pages.Add(spirePDF.Pages[i].Size, new Spire.Pdf.Graphics.PdfMargins(0), Spire.Pdf.PdfPageRotateAngle.RotateAngle0);
                                spirePDF.Pages[i].CreateTemplate().Draw(sPage, new PointF(0, 0));
                            }
                        }

                        DataSummitModels.Enums.Paper.Size psCur = DataSummitModels.DTO.Paper.Sizes.Match(pdfSingle.Pages[0].PageSize.Width, pdfSingle.Pages[0].PageSize.Height);
                        paperType = DataSummitModels.DTO.Paper.Sizes.All.FirstOrDefault(p => p.Size == psCur);
                        //Create image block blob
                        blockBlobJPG = blobContainer.GetBlockBlobReference("Original.jpg");

                        //Spire PDF image conversion
                        Image imgSpire = null;
                        try
                        {
                            //imgSpire = spdfSingle.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap, 200, 200);
                            MemoryStream msSpireJPG = (MemoryStream)spdfSingle.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap);
                            imgSpire.Save(msSpireJPG, ImageFormat.Jpeg);
                            msSpireJPG.Seek(0, SeekOrigin.Begin);
                            await blockBlobJPG.UploadFromStreamAsync(msSpireJPG);
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

                            Tasks.Add(new FunctionTask("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": pdf converted to jpg'", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                            log.LogInformation(imgUp.Tasks[Tasks.Count - 1].Name);

                            await blockBlobJPG.UploadFromByteArrayAsync(imgBytes, 0, imgBytes.Length);
                        }

                        Tasks.Add(new FunctionTask("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": uploaded to blob as 'Original.jpg'", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                        log.LogInformation("(Total: " + TimeSpan.FromTicks(Tasks.Sum(t => t.Duration.Ticks)).ToString(@"mm\:ss\.f") +
                                       " Delta: " + imgUp.Tasks[Tasks.Count - 1].Duration.ToString(@"mm\:ss\.f") + ") " +
                                       imgUp.Tasks[Tasks.Count - 1].Name);
                    }));

                    lTasks.Add(System.Threading.Tasks.Task.Run(async () =>
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

                        await blockBlobPDF.UploadFromStreamAsync(msSpirePDF);

                        //Populates layer names to a list from each document, which will allow unsupervised learning
                        if (spdfSingle.Layers.Count > 0)
                        {
                            for (int l = 0; l < spdfSingle.Layers.Count; l++)
                            {
                                lLayers.Add(spirePDF.Layers[l].Name);
                            }
                        }
                        lLayers.Distinct().OrderBy(l => l);

                        Tasks.Add(new FunctionTask("Page " + (i + 1).ToString() + " of " + pdfDoc.Pages.Count.ToString() + ": uploaded to blob as 'Original.pdf'", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                        log.LogInformation("(Total: " + TimeSpan.FromTicks(Tasks.Sum(t => t.Duration.Ticks)).ToString(@"mm\:ss\.f") +
                                           " Delta: " + imgUp.Tasks[Tasks.Count - 1].Duration.ToString(@"mm\:ss\.f") + ") " +
                                           imgUp.Tasks[Tasks.Count - 1].Name);
                    }));

                    //Ensure that all processes are finished
                    System.Threading.Tasks.Task.WaitAll(lTasks.ToArray());
                    Tasks.OrderBy(e => e.TimeStamp);

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
                        Tasks = Tasks.Select(t => t.ToModel(t)).ToList(),
                        Format = DataSummitModels.Enums.Document.Format.PDF,
                        Layers = new List<DocumentLayer>()
                        { 
                            new DocumentLayer()
                            {
                                Name = lLayers.First()
                            }
                        },
                        PaperSize = paperType,
                        UserId = imgUp.UserId
                    };

                    if (blockBlobJPG != null)
                    {
                        iu.StorageAccountName = blockBlobJPG.Name;
                        iu.StorageAccountKey = blockBlobJPG.Uri.ToString();
                        iu.BlobUrl = blockBlobJPG.StorageUri.ToString();
                    }
                    imgOut.Add(iu);
                }

                string jsonToReturn = JsonConvert.SerializeObject(imgOut);

                return new OkObjectResult(jsonToReturn);
            }
            catch (Exception ae)
            {
                //return error generated within function code
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ae));
            }
        }
    }
}
