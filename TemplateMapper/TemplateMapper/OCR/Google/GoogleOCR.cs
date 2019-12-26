using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
//using System.Web.Script.Serialization;
using TemplateMapper.OCR.Consolidated;
//using TemplateMapper.Extensions;
using TemplateMapper.OCR.Google.Request;
using TemplateMapper.OCR.Google.Response;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Models;

namespace TemplateMapper.OCR.Google
{
    [Serializable]
    public class GoogleOCR
    {
        public String FileName { get; set; }
        public Uri Uri { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Cloud Blobs { get; set; }

        public GoogleOCR()
        {
            //Blobs = new List<Cloud>();
        }

        public ConsolidatedOCR Run(List<System.Drawing.Image> images)
        {
            ConsolidatedOCR cOCR = new ConsolidatedOCR();
            try
            {
                Cloud gCloud = new Cloud();
                foreach (System.Drawing.Image i in images)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        i.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        m.Seek(0, SeekOrigin.Begin);
                        List<Responses> lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                        cOCR.FromGoogle(lrs, "");
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cOCR;
        }

        public ConsolidatedOCR Run(ImageUpload lDrawings)
        {
            ConsolidatedOCR cOCR = new ConsolidatedOCR();
            try
            {
                Cloud gCloud = new Cloud();
                foreach (ImageGrid ig in lDrawings.SplitImages)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        ig.Image.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        m.Seek(0, SeekOrigin.Begin);
                        List<Responses> lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                        cOCR.FromGoogle(lrs, ig.Name); 
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cOCR;
        }

        public ConsolidatedOCR Run(List<string> lfilepaths)
        {
            ConsolidatedOCR cOCR = new ConsolidatedOCR();
            try
            {
                Cloud gCloud = new Cloud();
                foreach (string s in lfilepaths)
                {
                    System.Drawing.Image i = System.Drawing.Image.FromFile(s);
                    using (MemoryStream m = new MemoryStream())
                    {
                        i.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        m.Seek(0, SeekOrigin.Begin);
                        List<Responses> lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                        cOCR.FromGoogle(lrs, s.Substring(s.LastIndexOf("/"),s.LastIndexOf(".")));
                    }
                }
                cOCR.FromGoogle(gCloud);
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cOCR;
        }

        public ConsolidatedOCR Run(List<Uri> lblobs)
        {
            ConsolidatedOCR cOCR = new ConsolidatedOCR();
            try
            {
                Cloud gCloud = new Cloud();

                //Azure cloud image data 
                string connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                            Keys.Azure.BlobStorageName, Keys.Azure.BlobStorageKey);
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();

                foreach (Uri u in lblobs)
                {
                    ICloudBlob b = blobClient.GetBlobReferenceFromServer(u);

                    byte[] imageBytes = new byte[b.Properties.Length];
                    b.DownloadRangeToByteArray(imageBytes, 0, 0, b.Properties.Length);
                    List<Responses> rs = RecogniseText(Convert.ToBase64String(imageBytes));
                    if (rs != null)
                    { gCloud.responses.AddRange(rs); }
                }
                cOCR.FromGoogle(gCloud);
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cOCR;
        }

        private List<Responses> RecogniseText(String ImageContent)
        {
            try
            {
                //Creates and populates bosepoke Google TextRecognition JSON object
                Requests r = new Requests();
                ImageAndFeat iaf = new ImageAndFeat();
                iaf.features.Add(new Features("TEXT_DETECTION"));
                iaf.image.content = ImageContent;
                r.requests.Add(iaf);
                String json = r.ToString(); // ToJSON();

                //Google's literal version:  Uri uri = new Uri("POST https://vision.googleapis.com/v1/images:annotate?key=AIzaSyDMe0LtaxFvFvDCTaEV-05IT792tvxpmbA");
                Uri uri = new Uri("https://vision.googleapis.com/v1/images:annotate?key=" + Keys.Google.API_Key);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                //req1.Timeout = 3600;
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = json.Length;
                //req.Headers.Add("x-functions-key", "i7W5DKPh3VldO/Taqc3vam/VN9xSN6bnwaJ6Dl0nolNcAPPMrYleeg==");
                //req.Headers.Add("x-functions-clientid", "default");
                //List<String> lHead = new List<string>();
                //for (int i = 0; i < req1.Headers.Count; i++)
                //{ lHead.Add(string.Join(":", req.Headers.GetValues(i).ToList())); }

                Stream stream = req.GetRequestStream();
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                //await stream.WriteAsync(buffer, 0, buffer.Length);

                //HttpWebResponse resp = default(HttpWebResponse);
                WebResponse resp = default(WebResponse);

                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                    //resp = req.GetResponse();
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
                //HttpStatusCode ht = resp.StatusCode;
                string responseText = String.Empty;
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd();
                    //responseText = await reader.ReadToEndAsync();
                }

                Cloud res = JsonConvert.DeserializeObject<Cloud>(responseText);
                
                return res.responses.ToList();
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return null;
        }
    }
}
