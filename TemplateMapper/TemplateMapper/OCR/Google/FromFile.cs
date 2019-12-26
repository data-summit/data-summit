using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TemplateMapper.OCR.Google.Response;

namespace TemplateMapper.OCR.Google
{
    public class FromFile
    {
        public OCR.Consolidated.ConsolidatedOCR Path(String FilePath)
        {
            List<GoogleOCR> lC = new List<GoogleOCR>();
            OCR.Consolidated.ConsolidatedOCR cOCR = new Consolidated.ConsolidatedOCR();
            
            try
            {
                String json = File.ReadAllText(FilePath);
                lC = JsonConvert.DeserializeObject<List<GoogleOCR>>(json);

                foreach(GoogleOCR g in lC)
                {
                    //Cloud c = g.Blobs;
                    cOCR.FileName = g.FileName;
                    cOCR.FromGoogle(g.Blobs);
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
