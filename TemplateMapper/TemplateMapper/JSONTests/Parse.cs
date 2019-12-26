using System;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Consolidated;

namespace TemplateMapper.JSONTests
{
    public static class Parse
    {
        public static ImageUpload JSONFile()
        {
            ImageUpload imgG = new ImageUpload();
            try
            {
                imgG = FromFile(@"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\ImageUpload Data (after Divide Function App).json");
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return imgG;
        }

        public static ImageUpload ImageUploadJson(string FilePath)
        {
            ImageUpload imgG = new ImageUpload();
            try
            {
                imgG = FromFile(FilePath);
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return imgG;
        }

        public static List<Sentence> SentencesJson(string FilePath)
        {
            List<Sentence> ls = new List<Sentence>();
            try
            {
                string json = "";

                if (File.Exists(FilePath) == true)
                {
                    using (StreamReader r = new StreamReader(FilePath))
                    { json = r.ReadToEnd(); }
                }
                ls = JsonConvert.DeserializeObject<List<Sentence>>(json);
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return ls;
        }

        private static ImageUpload FromFile(string FileName)
        {
            ImageUpload imgG = new ImageUpload();
            try
            {
                string json = "";
                
                if (File.Exists(FileName) == true)
                {
                    using (StreamReader r = new StreamReader(FileName))
                    { json = r.ReadToEnd(); }
                }
                imgG = JsonConvert.DeserializeObject<ImageUpload>(json);
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return imgG;
        }
    }
}
