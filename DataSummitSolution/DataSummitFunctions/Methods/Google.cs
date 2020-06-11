using DataSummitFunctions.Models;
using DataSummitFunctions.Models.Google.Request;
using DataSummitFunctions.Models.Google.Response;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DataSummitFunctions.Methods
{
    public class Google
    {
        public Drawing Run(List<System.Drawing.Image> images)
        {
            Drawing cOCR = new Drawing();
            try
            {
                Cloud gCloud = new Cloud();
                OCRSources ocrMethods = new OCRSources();
                foreach (System.Drawing.Image i in images)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        i.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        m.Seek(0, SeekOrigin.Begin);
                        List<Responses> lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                        ocrMethods.FromGoogle(lrs, "");
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return cOCR;
        }

        public Drawing Run(List<ImageGrid> lDrawings)
        {
            Drawing cOCR = new Drawing();
            try
            {
                Cloud gCloud = new Cloud();
                OCRSources ocrMethods = new OCRSources();
                foreach (ImageGrid ig in lDrawings)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        ig.Image.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        m.Seek(0, SeekOrigin.Begin);
                        List<Responses> lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                        ocrMethods.FromGoogle(lrs, ig.Name);
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return cOCR;
        }

        public Drawing Run(List<string> lfilepaths)
        {
            Drawing cOCR = new Drawing();
            try
            {
                Cloud gCloud = new Cloud();
                OCRSources ocrMethods = new OCRSources();
                foreach (string s in lfilepaths)
                {
                    System.Drawing.Image i = System.Drawing.Image.FromFile(s);
                    using (MemoryStream m = new MemoryStream())
                    {
                        i.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        m.Seek(0, SeekOrigin.Begin);
                        List<Responses> lrs = RecogniseText(Convert.ToBase64String(imageBytes));
                        ocrMethods.FromGoogle(lrs, s.Substring(s.LastIndexOf("/"), s.LastIndexOf(".")));
                    }
                }
                ocrMethods.FromGoogle(gCloud);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return cOCR;
        }

        public Drawing Run(List<Uri> lblobs)
        {
            Drawing cOCR = new Drawing();
            try
            {
                Cloud gCloud = new Cloud();
                OCRSources ocrMethods = new OCRSources();

                //Azure cloud image data 
                string connectionString = "";

                //String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                //                                            Keys.Azure.StorageAccountName, Keys.Azure.StorageAccountKey);
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
                ocrMethods.FromGoogle(gCloud);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
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
                String json = JsonConvert.SerializeObject(r);

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
                string strError = ae.Message.ToString();
            }
            return null;
        }

    }
}
