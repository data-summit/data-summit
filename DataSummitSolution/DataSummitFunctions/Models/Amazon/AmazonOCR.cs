using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using AWS = Amazon;

using System;
using System.IO;
using System.Linq;

namespace DataSummitFunctions.Models.Amazon
{
    public class AmazonOCR
    {
        public String FileName { get; set; }
        public Uri Uri { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Rekognition Results { get; set; }

        public AmazonOCR()
        { }

        public string Run(System.Drawing.Image ImageData)
        {
            var res = string.Empty;
            try
            {
                AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(
                    "",//DataSummitFunctions.Keys.Amazon.AccessKeyID, 
                    "");//DataSummitFunctions.Keys.Amazon.SecretAccessKey, AWS.RegionEndpoint.EUWest1);
                DetectTextRequest detectTextRequest = new DetectTextRequest();

                var json = string.Empty;
                var m = new MemoryStream();
                ImageData.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                m.Position = 0;

                detectTextRequest.Image = new Image
                {
                    Bytes = m
                };
                var detectTextResponse = rekognitionClient.DetectText(detectTextRequest);

                var r = detectTextResponse.TextDetections.Select(f => f.DetectedText).Distinct().ToList().First();
                res = string.Join("", detectTextResponse.TextDetections.SelectMany(f => f.DetectedText).Distinct().ToList());

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
