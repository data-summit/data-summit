using AzureFunctions.Models;
using AzureFunctions.Models.Google.Request;
using AzureFunctions.Models.Google.Response;
using DataSummitModels.DB;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Methods
{
    public class Google
    {
        public Document Run(List<System.Drawing.Image> images)
        {
            var cOCR = new Document();
            try
            {
                var gCloud = new Cloud();
                var ocrMethods = new OCRSources();
                foreach (System.Drawing.Image i in images)
                {
                    using MemoryStream m = new MemoryStream();
                    i.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = m.ToArray();
                    m.Seek(0, SeekOrigin.Begin);
                    var lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                    ocrMethods.FromGoogle(lrs, "");
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message;
            }
            return cOCR;
        }

        public Document Run(List<ImageGrids> lDocuments)
        {
            var cOCR = new Document();
            try
            {
                var gCloud = new Cloud();
                var ocrMethods = new OCRSources();
                foreach (ImageGrids ig in lDocuments)
                {
                    using MemoryStream m = new MemoryStream();
                    ig.Image.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = m.ToArray();
                    m.Seek(0, SeekOrigin.Begin);
                    var lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                    ocrMethods.FromGoogle(lrs, ig.Name);
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message;
            }
            return cOCR;
        }

        public Document Run(List<string> lfilepaths)
        {
            var cOCR = new Document();
            try
            {
                var gCloud = new Cloud();
                var ocrMethods = new OCRSources();
                foreach (string s in lfilepaths)
                {
                    System.Drawing.Image i = System.Drawing.Image.FromFile(s);
                    using MemoryStream m = new MemoryStream();
                    i.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = m.ToArray();
                    m.Seek(0, SeekOrigin.Begin);
                    var lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                    ocrMethods.FromGoogle(lrs, s.Substring(s.LastIndexOf("/"), s.LastIndexOf(".")));
                }
                ocrMethods.FromGoogle(gCloud);
            }
            catch (Exception ae)
            {
                string strError = ae.Message;
            }
            return cOCR;
        }

        public async Task<Document> RunAsync(List<Uri> lblobs)
        {
            var cOCR = new Document();
            try
            {
                var gCloud = new Cloud();
                var ocrMethods = new OCRSources();

                //Azure cloud image data 
                string connectionString = "";

                var account = CloudStorageAccount.Parse(connectionString);
                var blobClient = account.CreateCloudBlobClient();

                foreach (Uri u in lblobs)
                {
                    var b = await blobClient.GetBlobReferenceFromServerAsync(u);

                    var imageBytes = new byte[b.Properties.Length];
                    await b.DownloadRangeToByteArrayAsync(imageBytes, 0, 0, b.Properties.Length);
                    var rs = RecogniseText(Convert.ToBase64String(imageBytes));
                    if (rs != null)
                    { gCloud.responses.AddRange(rs); }
                }
                ocrMethods.FromGoogle(gCloud);
            }
            catch (Exception ae)
            {
                string strError = ae.Message;
            }
            return cOCR;
        }

        private List<Responses> RecogniseText(String ImageContent)
        {
            try
            {
                //Creates and populates bosepoke Google TextRecognition JSON object
                var r = new Requests();
                var iaf = new ImageAndFeat();
                iaf.features.Add(new Features("TEXT_DETECTION"));
                iaf.image.content = ImageContent;
                r.requests.Add(iaf);
                var json = JsonConvert.SerializeObject(r);

                //Google's literal version:  Uri uri = new Uri("POST https://vision.googleapis.com/v1/images:annotate?key=AIzaSyDMe0LtaxFvFvDCTaEV-05IT792tvxpmbA");
                var uri = new Uri("https://vision.googleapis.com/v1/images:annotate?key=" + "Keys.Google.API_Key"); //Replace 'Keys.Google.API_Key' with Azure Secret of actual key

                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = json.Length;

                var stream = req.GetRequestStream();
                var buffer = Encoding.UTF8.GetBytes(json);
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();

                var resp = default(WebResponse);
                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)wex.Response)
                        {
                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                            {
                                string error = reader.ReadToEnd();

                                //TODO: use JSON.net to parse this string and look at the error message
                            }
                        }
                    }
                }
                string responseText = String.Empty;
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd();
                }

                var res = JsonConvert.DeserializeObject<Cloud>(responseText);

                return res.responses.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }

            return null;
        }
    }
}
