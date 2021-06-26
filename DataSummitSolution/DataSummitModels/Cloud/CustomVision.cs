
using System.Text.RegularExpressions;

namespace DataSummitModels.Cloud
{
    public class CustomVision
    {
        public string BlobUrl { get; set; }
        public string MLUrl { get; set; }
        public string TrainingKey { get; set; }
        public string PredictionKey { get; set; }
        public string MLProjectName { get; set; }
        public double MinThreshold { get; set; } = 0.95;

        /// <summary>
        /// Return the number 1323 from an example string
        /// https://a.b.c.com/customvision/v3.1/Prediction/<GUID>/classify/iterations/Iteration1323/url
        /// </summary>
        public string TrainingIteration
        {
            get
            {
                var regex = new Regex(@"(?<=\/Iteration)\d+(?=\/)");
                var match = regex.Match(MLUrl);
                return match.Value;
            }
        }
    }
}