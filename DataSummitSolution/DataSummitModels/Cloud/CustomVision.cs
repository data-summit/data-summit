
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

        public CustomVision()
        { ; }

        public string GetIteration()
        {
            // Return the number 1323 from an example string http://test/Iteration1323/
            var rgx = new Regex(@"(?!Iteration)(\d+)(?=\/)");
            var match = rgx.Match(MLUrl);
            return match.Value;
        }
    }
}