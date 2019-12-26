using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Amazon
{
    public class FromFile
    {
        public OCR.Consolidated.ConsolidatedOCR Path(String FilePath, List<ImageGrid> Drawings)
        {
            List<AmazonOCR> lC = new List<AmazonOCR>();
            OCR.Consolidated.ConsolidatedOCR cOCR = new Consolidated.ConsolidatedOCR();
            try
            {
                String json = File.ReadAllText(FilePath);
                lC = JsonConvert.DeserializeObject<List<AmazonOCR>>(json);
                
                foreach(AmazonOCR a in lC)
                {
                    Rekognition am = a.Results;
                    cOCR.FileName = a.FileName;
                    cOCR.FromAmazon(a, Drawings);
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return cOCR;
        }
    }
}
