using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using AWS = Amazon;

using System;
using System.IO;
using System.Linq;

namespace AzureFunctions.Models.Amazon
{
    public class AmazonOCR
    {
        public string FileName { get; set; }
        public Uri Uri { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Rekognition Results { get; set; }

        public AmazonOCR()
        { }

        public async System.Threading.Tasks.Task<string> RunAsync(System.Drawing.Image ImageData)
        {
            String res = String.Empty;
            try
            {
                AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(
                    AzureFunctions.Keys.Amazon.AccessKeyID, 
                    AzureFunctions.Keys.Amazon.SecretAccessKey, AWS.RegionEndpoint.EUWest1);
                DetectTextRequest detectTextRequest = new DetectTextRequest();

                String json = String.Empty;
                MemoryStream m = new MemoryStream();
                ImageData.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                m.Position = 0;

                detectTextRequest.Image = new AWS.Rekognition.Model.Image();
                detectTextRequest.Image.Bytes = m;
                DetectTextResponse detectTextResponse = await rekognitionClient.DetectTextAsync(detectTextRequest);

                var r = detectTextResponse.TextDetections.Select(f => f.DetectedText).Distinct().ToList().First();
                res = String.Join("", detectTextResponse.TextDetections.SelectMany(f => f.DetectedText).Distinct().ToList());

                //using (StreamReader file = new StreamReader(strAmazonTestOCRResData))
                //{
                //    json = file.ReadToEnd();
                //}

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //AmazonOCR res = serializer.Deserialize<AmazonOCR>(detectTextResponse);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return res;
        }
    }
}
