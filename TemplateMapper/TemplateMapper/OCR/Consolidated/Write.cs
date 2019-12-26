using Newtonsoft.Json;
using System;
using System.IO;

namespace TemplateMapper.OCR.Consolidated
{
    public static class Write
    {
        public static void ToJSON(ConsolidatedOCR cOCR, string filepath)
        {
            try
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(cOCR));
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }
    }
}
