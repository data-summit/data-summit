using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Azure
{
    public class FromFile
    {
        public OCR.Consolidated.ConsolidatedOCR Path(String FilePath)
        {
            List<BlobOCR> bC = new List<BlobOCR>();
            OCR.Consolidated.ConsolidatedOCR cOCR = new Consolidated.ConsolidatedOCR();
            try
            {
                String json = File.ReadAllText(FilePath);
                bC = JsonConvert.DeserializeObject<List<BlobOCR>>(json);

                foreach (BlobOCR b in bC)
                {
                    AzureOCR az = (AzureOCR)b.Results;
                    cOCR.FileName = b.FileName;
                    cOCR.FromAzure(az);
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
