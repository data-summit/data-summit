using DataSummitFunctions.Models;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json;

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
    public static class DivideImage
    {
        public static int MaxPixelSpan = 2200;

        [FunctionName("DivideImage")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                                            HttpRequestMessage req, TraceWriter log)// Microsoft.Extensions.Logging.ILogger log)
        {
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload imgUp = (ImageUpload)data;
                //ImageUpload img = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);

                if (imgUp.Tasks == null) imgUp.Tasks = new List<Tasks>();
                if (imgUp.Layers == null) imgUp.Layers = new List<string>();

                //if (imgUp.CompanyId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: CompanyId is less than zero.");
                //if (imgUp.ProjectId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: ProjectId is less than zero.");
                if (imgUp.DrawingId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: DrawingId is less than zero.");
                //if (imgUp.Company == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Company is blank.");
                //if (imgUp.Project == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Project is blank.");
                if (imgUp.FileName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: File name is ,less than zero.");
                if (imgUp.Type == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Type is blank.");
                if (imgUp.StorageAccountName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Storage name required.");
                if (imgUp.StorageAccountKey == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Storage key required.");
                if (imgUp.WidthOriginal <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Image must have width greater than zero");
                if (imgUp.HeightOriginal <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Image must have height greater than zero");
                if (imgUp.ContainerName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Container must have a GUID name");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.StorageAccountName +
                                           ";AccountKey=" + imgUp.StorageAccountKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Blob connection";
                if (account != null) { log.Info(strError + ": failed"); }
                else { log.Info(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.Info(strError + ": failed"); }
                else { log.Info(strError + " = " + blobClient.ToString() + ": success"); }

                if (imgUp.Tasks.Count == 0)
                { imgUp.Tasks.Add(new Tasks("Divide Images\tGet container", DateTime.Now)); }
                else
                { imgUp.Tasks.Add(new Tasks("Divide Images\tGet container", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp)); }

                //Get Container name from input object, exit if not found
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);
                if (cbc.Exists() == false) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Cannot find Container with name: " + imgUp.ContainerName);

                //Find 'Original.jpg' blob
                CloudBlockBlob cbbOrig = default(CloudBlockBlob);
                if (cbc.ListBlobs().Count(b => b.Uri.ToString() == imgUp.BlobURL) > 0)
                {
                    cbbOrig = cbc.ListBlobs().Cast<CloudBlockBlob>().FirstOrDefault(b => b.Uri.ToString() == imgUp.BlobURL);
                }
                else
                {
                    foreach (IListBlobItem blobItem in cbc.ListBlobs())
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

                imgUp.Tasks.Add(new Tasks("Fetch 'Original.jpg'", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                if (cbbOrig == null)
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Could not locate 'Original.jpg' file in container '" + imgUp.ContainerName + "'.");
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    cbbOrig.DownloadToStream(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    Image img = Image.FromStream(ms);

                    //Split images prior to upload
                    if (imgUp.SplitImages == null) imgUp.SplitImages = new List<ImageGrid>();

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
                            ImageGrid ig = new ImageGrid
                            {
                                Name = "F_" + x.ToString("000") + "-" + y.ToString("000") + ".jpg",
                                WidthStart = x,
                                HeightStart = y,
                                Type = ImageGrid.ImageType.Normal
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
                            ImageGrid ig = new ImageGrid
                            {
                                Name = "O_" + x.ToString("000") + "-" + y.ToString("000") + ".jpg",
                                WidthStart = x,
                                HeightStart = y,
                                Type = ImageGrid.ImageType.Overlap
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
                                                     imgUp.SplitImages.Count(d => d.Type == ImageGrid.ImageType.Overlap).ToString() + 
                                                     " overlapping images", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                    foreach (ImageGrid imgG in imgUp.SplitImages)
                    {
                        CloudBlockBlob gridBlob = cbc.GetBlockBlobReference(imgG.Name);
                        MemoryStream msG = new MemoryStream();
                        imgG.Image.Save(msG, System.Drawing.Imaging.ImageFormat.Jpeg);
                        msG.Seek(0, SeekOrigin.Begin);
                        gridBlob.UploadFromStream(msG);

                        imgG.BlobURL = gridBlob.Uri.ToString();
                        gridBlob.Metadata.Add("Name", imgG.Name);
                        gridBlob.Metadata.Add("WidthStart", imgG.WidthStart.ToString());
                        gridBlob.Metadata.Add("HeightStart", imgG.HeightStart.ToString());
                        gridBlob.Metadata.Add("Width", imgG.Width.ToString());
                        gridBlob.Metadata.Add("Height", imgG.Height.ToString());
                        gridBlob.Metadata.Add("Type", imgG.Type.ToString());
                        gridBlob.SetMetadata();

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
                jsonBlob.UploadText(JsonConvert.SerializeObject(imgUp));

                imgUp.Tasks.Add(new Tasks("Divide Image\tFunction complete", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                string jsonToReturn = JsonConvert.SerializeObject(imgUp);

                return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json") };

            }
            catch (Exception ae)
            {
                //return error generated within function code
                HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.BadRequest);
                res.Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json");
                return res;
            }
        }
    }
}

