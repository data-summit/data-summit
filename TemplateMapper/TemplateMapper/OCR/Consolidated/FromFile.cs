using Newtonsoft.Json;
using System;
using System.IO;

namespace TemplateMapper.OCR.Consolidated
{
    public class FromFile
    {
        public ConsolidatedOCR Read(string filepath)
        {
            ConsolidatedOCR cOCR = new ConsolidatedOCR();
            try
            {
                if (File.Exists(filepath))
                {
                    using (StreamReader r = new StreamReader(filepath))
                    {
                        cOCR = JsonConvert.DeserializeObject<ConsolidatedOCR>(r.ReadToEnd());
                        cOCR.FileName = filepath.Substring(filepath.LastIndexOf("\\") + 1, filepath.Length - filepath.LastIndexOf("\\") - 1);
                    }
                    //using (StreamReader r = new StreamReader(filepath))
                    //{
                    //    var c = JsonConvert.DeserializeObject(r.ReadToEnd());
                    //}
                }
                else
                {
                    return null;
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
