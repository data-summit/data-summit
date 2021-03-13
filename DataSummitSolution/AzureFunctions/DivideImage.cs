using DataSummitModels.DB;
using DataSummitModels.Enums;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public static class DivideImage
    {
        public static int MaxPixelSpan = 2200;

        [FunctionName("DivideImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload imgUp = (ImageUpload)data;
                //ImageUpload img = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);

                if (imgUp.Tasks == null) imgUp.Tasks = new List<Tasks>();
                if (imgUp.Layers == null) imgUp.Layers = new List<DocumentLayers>();

                //if (imgUp.CompanyId < 0) return new BadRequestObjectResult("Illegal input: CompanyId is less than zero.");
                //if (imgUp.ProjectId < 0) return new BadRequestObjectResult("Illegal input: ProjectId is less than zero.");
                if (imgUp.DocumentId < 0) return new BadRequestObjectResult("Illegal input: DocumentId is less than zero.");
                //if (imgUp.Company == "") return new BadRequestObjectResult("Illegal input: Company is blank.");
                //if (imgUp.Project == "") return new BadRequestObjectResult("Illegal input: Project is blank.");
                if (imgUp.FileName == "") return new BadRequestObjectResult("Illegal input: File name is ,less than zero.");
                //if (imgUp.Type == DataSummitModels.Enums.Document.Type.Unknown) return new BadRequestObjectResult("Illegal input: Type is blank.");
                if (imgUp.StorageAccountName == "") return new BadRequestObjectResult("Illegal input: Storage name required.");
                if (imgUp.StorageAccountKey == "") return new BadRequestObjectResult("Illegal input: Storage key required.");
                if (imgUp.WidthOriginal <= 0) return new BadRequestObjectResult("Illegal input: Image must have width greater than zero");
                if (imgUp.HeightOriginal <= 0) return new BadRequestObjectResult("Illegal input: Image must have height greater than zero");
                if (imgUp.ContainerName == "") return new BadRequestObjectResult("Illegal input: Container must have a GUID name");

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

                if (imgUp.Tasks.Count == 0)
                { imgUp.Tasks.Add(new Tasks("Divide Images\tGet container", DateTime.Now)); }
                else
                { imgUp.Tasks.Add(new Tasks("Divide Images\tGet container", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp)); }

                //Get Container name from input object, exit if not found
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);
                if (await cbc.ExistsAsync() == false)
                { return new BadRequestObjectResult("Illegal input: Cannot find Container with name: " + imgUp.ContainerName); }
                
                //Find 'Original.jpg' blob
                CloudBlockBlob cbbOrig = default(CloudBlockBlob);
                BlobContinuationToken blobContinuationToken = null;
                var listBlobs = await cbc.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = listBlobs.ContinuationToken;
                do
                {
                    if (listBlobs.Results.Count(b => b.Uri.ToString() == imgUp.BlobUrl) > 0)
                    {
                        cbbOrig = listBlobs.Results.Cast<CloudBlockBlob>().FirstOrDefault(b => b.Uri.ToString() == imgUp.BlobUrl);
                    }
                    else
                    {
                        foreach (IListBlobItem blobItem in listBlobs.Results)
                        {
                            if (blobItem is CloudBlockBlob)
                            {
                                CloudBlockBlob cbb = blobItem as CloudBlockBlob;

                                if (cbb.Name == "Original.jpg")
                                {
                                    cbbOrig = cbb;
                                    break;
                                }
                            }
                        }
                    }
                } while (blobContinuationToken != null);    // Loop while the continuation token is not null.

                imgUp.Tasks.Add(new Tasks("Fetch 'Original.jpg'", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                if (cbbOrig == null)
                {
                    return new BadRequestObjectResult("Could not locate 'Original.jpg' file in container '" + imgUp.ContainerName + "'.");
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    await cbbOrig.DownloadToStreamAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                    //Split images prior to upload
                    if (imgUp.SplitImages == null) imgUp.SplitImages = new List<ImageGrids>();

                    int widthMod = (int)Math.Ceiling(((double)img.Width / MaxPixelSpan));
                    int heightMod = (int)Math.Ceiling(((double)img.Height / MaxPixelSpan));

                    int widthSpan = (img.Width / widthMod);
                    int heightSpan = (img.Height / heightMod);

                    int widthFinalAdjust = img.Width % (widthSpan * widthMod);
                    int heightFinalAdjust = img.Height % (heightSpan * heightMod);

                    Bitmap bmp = img as Bitmap;
                    bmp.SetResolution(img.Width, img.Height);

                    //Create normal grid system
                    for (int x = 0; x < widthMod; x++)
                    {
                        for (int y = 0; y < heightMod; y++)
                        {
                            ImageGrids ig = new ImageGrids
                            {
                                Name = "F_" + x.ToString("000") + "-" + y.ToString("000") + ".jpg",
                                WidthStart = x,
                                HeightStart = y,
                                Type = DataSummitModels.Enums.Image.Type.Normal
                            };
                            if (x == widthMod - 1 && y != heightMod - 1)
                            {
                                ig.WidthStart = x * widthSpan;
                                ig.HeightStart = y * heightSpan;
                                ig.Width = widthSpan + widthFinalAdjust;
                                ig.Height = heightSpan;
                                ig.Image = bmp.Clone(new Rectangle(ig.WidthStart, ig.HeightStart, ig.Width, ig.Height), PixelFormat.Format24bppRgb);
                            }
                            else if (x != widthMod - 1 && y == heightMod - 1)
                            {
                                ig.WidthStart = x * widthSpan;
                                ig.HeightStart = y * heightSpan;
                                ig.Width = widthSpan;
                                ig.Height = heightSpan + heightFinalAdjust;
                                ig.Image = bmp.Clone(new Rectangle(ig.WidthStart, ig.HeightStart, ig.Width, ig.Height), PixelFormat.Format24bppRgb);
                            }
                            else if (x == widthMod - 1 && y == heightMod - 1)
                            {
                                ig.WidthStart = x * widthSpan;
                                ig.HeightStart = y * heightSpan;
                                ig.Width = widthSpan + widthFinalAdjust;
                                ig.Height = heightSpan + heightFinalAdjust;
                                ig.Image = bmp.Clone(new Rectangle(ig.WidthStart, ig.HeightStart, ig.Width, ig.Height), PixelFormat.Format24bppRgb);
                            }
                            else
                            {
                                ig.WidthStart = x * widthSpan;
                                ig.HeightStart = y * heightSpan;
                                ig.Width = widthSpan;
                                ig.Height = heightSpan;
                                ig.Image = bmp.Clone(new Rectangle(ig.WidthStart, ig.HeightStart, ig.Width, ig.Height), PixelFormat.Format24bppRgb);
                            }

                            imgUp.SplitImages.Add(ig);
                        }
                    }

                    imgUp.Tasks.Add(new Tasks("Divide Image\tOriginal divide into " + imgUp.SplitImages.Count().ToString() + " adjoining images", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                    //Create overlap grid system
                    for (int x = 0; x < widthMod; x++)
                    {
                        for (int y = 0; y < heightMod; y++)
                        {
                            ImageGrids ig = new ImageGrids
                            {
                                Name = "O_" + x.ToString("000") + "-" + y.ToString("000") + ".jpg",
                                WidthStart = x,
                                HeightStart = y,
                                Type = DataSummitModels.Enums.Image.Type.Overlap
                            };
                            ig.WidthStart = (x * widthSpan) + (widthSpan / 2);
                            ig.HeightStart = (y * heightSpan) + (heightSpan / 2);
                            ig.Width = widthSpan;
                            ig.Height = heightSpan;
                            if ((ig.WidthStart + ig.Width) < img.Width && (ig.HeightStart + ig.Height) < img.Height)
                            {
                                ig.Image = bmp.Clone(new Rectangle(ig.WidthStart, ig.HeightStart, ig.Width, ig.Height), PixelFormat.Format24bppRgb);
                                imgUp.SplitImages.Add(ig);
                            }
                        }
                    }

                    imgUp.Tasks.Add(new Tasks("Divide Image\tOriginal divide into " +
                                                     imgUp.SplitImages.Count(d => d.Type == DataSummitModels.Enums.Image.Type.Overlap).ToString() +
                                                     " overlapping images", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                    foreach (ImageGrids imgG in imgUp.SplitImages)
                    {
                        CloudBlockBlob gridBlob = cbc.GetBlockBlobReference(imgG.Name);
                        MemoryStream msG = new MemoryStream();
                        imgG.Image.Save(msG, ImageFormat.Jpeg);
                        msG.Seek(0, SeekOrigin.Begin);
                        await gridBlob.UploadFromStreamAsync(msG);

                        imgG.BlobUrl = gridBlob.Uri.ToString();
                        gridBlob.Metadata.Add("Name", imgG.Name);
                        gridBlob.Metadata.Add("WidthStart", imgG.WidthStart.ToString());
                        gridBlob.Metadata.Add("HeightStart", imgG.HeightStart.ToString());
                        gridBlob.Metadata.Add("Width", imgG.Width.ToString());
                        gridBlob.Metadata.Add("Height", imgG.Height.ToString());
                        gridBlob.Metadata.Add("Type", imgG.Type.ToString());
                        await gridBlob.SetMetadataAsync();

                        imgG.Image = null;

                        imgUp.Tasks.Add(new Tasks("Divide Image\tImage " +
                                                         (imgUp.SplitImages.IndexOf(imgG) + 1).ToString() + " uploaded",
                                                         imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                    }
                    bmp.Dispose();
                }

                imgUp.Tasks.Add(new Tasks("Divide Image\tCreating JSON Image Structure Text", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                //Write json data file to blob, containing the above information
                CloudBlockBlob jsonBlob = cbc.GetBlockBlobReference("Split Images Data & Structure.json");
                await jsonBlob.UploadTextAsync(JsonConvert.SerializeObject(imgUp));

                imgUp.Tasks.Add(new Tasks("Divide Image\tFunction complete", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                string jsonToReturn = JsonConvert.SerializeObject(imgUp);

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