using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Azure
{
    public class AzureOCR
    {
        public String Language { get; set; }
        public String Orientation { get; set; }
        public List<Region> Regions = new List<Region>();
        public Double? TextAngle { get; set; }

        public AzureOCR()
        { }

        public async Task Run2()
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", Keys.Azure.VisionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = Keys.Azure.VisionUri + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                string connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                            Keys.Azure.BlobStorageName, Keys.Azure.BlobStorageKey);

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                String ContainerName = "00000015";
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(ContainerName);
                CloudBlockBlob blockBlobOrig = blobContainer.GetBlockBlobReference("F_000-000.jpg");
                blockBlobOrig.FetchAttributes();

                byte[] byteData = new byte[blockBlobOrig.Properties.Length];
                blockBlobOrig.DownloadToByteArray(byteData, 0);
                

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                OcrResult o = JsonConvert.DeserializeObject<OcrResult>(contentString);
                //// Display the JSON response.
                //Console.WriteLine("\nResponse:\n\n{0}\n",
                //    JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        public async Task<String> Run()
        {
            String sOut = String.Empty;
            try
            {
                ComputerVisionClient cvc = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(Keys.Azure.VisionKey),
                    new System.Net.Http.DelegatingHandler[] { });

                cvc.Endpoint = Keys.Azure.VisionUri;

                TemplateMapper.OCR.Azure.BlobOCR o = new TemplateMapper.OCR.Azure.BlobOCR();
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = null;
                OcrResult res = null;

                string connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                            Keys.Azure.BlobStorageName, Keys.Azure.BlobStorageKey);

                String strError = String.Empty;

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                String ContainerName = "00000015";
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(ContainerName);
                //CloudBlockBlob blockBlobOrig = blobContainer.GetBlockBlobReference("Original.jpg");
                CloudBlockBlob blockBlobOrig = blobContainer.GetBlockBlobReference("F_000-000.jpg"); 

                //MemoryStream ms = new MemoryStream();
                //blockBlobOrig.DownloadToStream(ms);
                //ms.Seek(0, SeekOrigin.Begin);
                //Task<OcrResult> task = Task.Run(async () => res = await cvc.RecognizePrintedTextInStreamAsync(true, ms));

                Task<OcrResult> task = Task.Run(async () => res = await cvc.RecognizePrintedTextAsync(false, blockBlobOrig.Uri.ToString()));
                task.Wait();

                if (res != null)
                {
                    o.Results.Language = res.Language;
                    o.Results.Orientation = res.Orientation;
                    o.Results.TextAngle = (double)res.TextAngle;
                    foreach (OcrRegion r in res.Regions.ToList())
                    { o.Results.Regions.Add(Region.CastTo(r)); }
                    sOut = String.Join(" ", o.Results.Regions.SelectMany(r => r.Lines.SelectMany(l => l.Words.Select(y => y.Text))));
                    //var ge2 = res.Regions.Select(e => e.Lines.Select(fo => String.Join(" ", fo.Words.Select(go => go.Text).ToList()).ToList()).ToList()).ToList();
                    //var ge3 = res.Regions.Select(e => e.Lines.SelectMany(fo => String.Join(" ", fo.Words.Select(go => go.Text).ToList()).ToList()).ToList()).ToList();
                    //foreach(Region r in o.Results.Regions)
                    //{
                    //    var ge5 = String.Join(" ", r.Lines.SelectMany(l => l.Words.Select(y => y.Text)));
                    //}
                    Regions = o.Results.Regions.ToList();
                }
            }

            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return sOut;
        }

        public async Task<String> Run(Image ImageData, int borderSize)
        //public String Run(Image ImageData)
        {
            String sOut = String.Empty;
            try
            {
                //VisionServiceClient vsc = new VisionServiceClient(Keys.Azure.VisionKey,
                //                                                  Keys.Azure.VisionUri);
                //VisionServiceClient vsc = new VisionServiceClient(Keys.Azure.VisionKey);
                    
                TemplateMapper.OCR.Azure.BlobOCR o = new TemplateMapper.OCR.Azure.BlobOCR();
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = null;
                OcrResult res = null;

                string connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                            Keys.Azure.BlobStorageName, Keys.Azure.BlobStorageKey);

                String strError = String.Empty;

                //CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                //CloudBlobClient blobClient = account.CreateCloudBlobClient();
                //String ContainerName = "000temp";
                //CloudBlobContainer blobContainer = blobClient.GetContainerReference(ContainerName);
                //CloudBlockBlob blockBlobOrig = blobContainer.GetBlockBlobReference("Snippet.jpg");

                MemoryStream m = new MemoryStream();
                ImageData.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                m.Position = 0;
                byte[] dat = m.ToArray();

                //blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);
                //blockBlobOrig.UploadFromStream(m);
                String f = @"C:\Users\TJ\Downloads\Temp Image.jpg";
                if (File.Exists(f)) File.Delete(f);
                //Image i = System.Drawing.Image.FromStream(m);
                //i.Save(f, System.Drawing.Imaging.ImageFormat.Jpeg);
                Bitmap bmp = new Bitmap(m);

                //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
                //System.Drawing.Point pos = new System.Drawing.Point(borderSize, borderSize);
                //using (Brush border = new SolidBrush(System.Drawing.Color.White))
                //{
                //    //g.FillRectangle(border, 
                //    //                pos.X - borderSize, pos.Y - borderSize,
                //    //                bmp.Width + borderSize, bmp.Height + borderSize);
                //    g.FillRectangle(border, 0, 0, bmp.Width + (2 * borderSize), bmp.Height + (2 * borderSize));
                //}
                //g.DrawImage(bmp, pos);
                Bitmap bmpBlank = new Bitmap(bmp.Width + (2 * borderSize), bmp.Height + (2 * borderSize));
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpBlank);
                System.Drawing.Point pos = new System.Drawing.Point(borderSize, borderSize);
                using (Brush border = new SolidBrush(System.Drawing.Color.White))
                {
                    //g.FillRectangle(border, 
                    //                pos.X - borderSize, pos.Y - borderSize,
                    //                bmp.Width + borderSize, bmp.Height + borderSize);
                    g.FillRectangle(border, 0, 0, bmpBlank.Width, bmpBlank.Height);
                }
                //bmp.Save(f, System.Drawing.Imaging.ImageFormat.Jpeg);
                g.DrawImage(bmp, pos);
                Stream stream = new MemoryStream();
                bmpBlank.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Position = 0;

                //bmpBlank.Save(f, System.Drawing.Imaging.ImageFormat.Jpeg);
                //using (FileStream stream = File.Open(f, FileMode.Open))
                //{
                //    res = vsc.RecognizeTextAsync(stream, LanguageCodes.AutoDetect, true).Result;
                //}

                //res = cvc.RecognizeTextAsync(stream, LanguageCodes.AutoDetect, true).Result;

                //Untested on-the-fly conversion of objects
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = GetOCRResultsViaURI(blockBlobOrig.Uri, vsc);
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = GetOCRResultsViaMemoryStream(m, vsc);
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = GetOCRResultsViaStream(fs, vsc);
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = await vsc.RecognizeTextAsync(m);
                //Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults res = await vsc.RecognizeTextAsync(fs);
                //if (res != null)
                //{
                //    o.Results.Language = res.Language;
                //    o.Results.Orientation = res.Orientation;
                //    o.Results.TextAngle = (double)res.TextAngle;
                //    foreach (Microsoft.Azure.CognitiveServices.Vision.Contract.Region r in res.Regions.ToList())
                //    { o.Results.Regions.Add(Region.CastTo(r)); }
                //    sOut = String.Join(" ", o.Results.Regions.SelectMany(r => r.Lines.SelectMany(l => l.Words.Select(y => y.Text))));
                //    //var ge2 = res.Regions.Select(e => e.Lines.Select(fo => String.Join(" ", fo.Words.Select(go => go.Text).ToList()).ToList()).ToList()).ToList();
                //    //var ge3 = res.Regions.Select(e => e.Lines.SelectMany(fo => String.Join(" ", fo.Words.Select(go => go.Text).ToList()).ToList()).ToList()).ToList();
                //    //foreach(Region r in o.Results.Regions)
                //    //{
                //    //    var ge5 = String.Join(" ", r.Lines.SelectMany(l => l.Words.Select(y => y.Text)));
                //    //}
                //    Regions = o.Results.Regions.ToList();
                //}


                //fs.Close();
                //if (File.Exists(f)) File.Delete(f);
            }

            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return sOut;
        }

        //private OcrResults GetOCRResultsViaMemoryStream(MemoryStream s, VisionServiceClient VisionServiceClient)
        //{
        //    Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults o = new Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults();
        //    try
        //    {
        //        //Task.Run(async () => o = await VisionServiceClient.RecognizeTextAsync(s)).Wait();
        //        Task<Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults> t = Task.Run(async () => o = await VisionServiceClient.RecognizeTextAsync(s));
        //        t.Wait();
        //        var p = t.Result;
        //    }
        //    catch (Exception ae)
        //    {
        //        String strError = ae.ToString();
        //        return null;
        //    }
        //    return o;
        //}

        //private OcrResults GetOCRResultsViaStream(Stream s, VisionServiceClient VisionServiceClient)
        //{
        //    Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults o = new Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults();
        //    try
        //    {
        //        //Task.Run(async () => o = await VisionServiceClient.RecognizeTextAsync(s)).Wait();
        //        Task<Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults> t = Task.Run(async () => o = await VisionServiceClient.RecognizeTextAsync(s));
        //        t.Wait();
        //        var p = t.Result;
        //    }
        //    catch (Exception ae)
        //    {
        //        String strError = ae.ToString();
        //        return null;
        //    }
        //    return o;
        //}

        //private OcrResults GetOCRResultsViaURI(Uri uri, VisionServiceClient VisionServiceClient)
        //{
        //    Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults o = new Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults();
        //    try
        //    {
        //        Task<Microsoft.Azure.CognitiveServices.Vision.Contract.OcrResults> task = Task.Run(async () => o = await VisionServiceClient.RecognizeTextAsync(uri.ToString()));
        //        task.Wait();
        //    }
        //    catch (Exception ae)
        //    {
        //        String strError = ae.ToString();
        //        return null;
        //    }
        //    return o;
        //}
    }
}
