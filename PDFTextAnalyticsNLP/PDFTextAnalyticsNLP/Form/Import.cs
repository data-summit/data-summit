using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PDFTextAnalyticsNLP.Form
{
    public class Import
    {
        public string OpenPDF(string sInitial)
        {
            string sOut = "";
            try
            {
                OpenFileDialog o = new OpenFileDialog
                {
                    InitialDirectory = sInitial,
                    Multiselect = false,
                    Title = "Open PDF Table",
                    Filter = "Adobe PDF Files (*.pdf)|*.pdf"
                };

                if (o.ShowDialog() == DialogResult.OK)
                {
                    sOut = o.FileNames[0];
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
            return sOut;
        }
        public List<Document> ReadFile(string fileName)
        {
            List<Document> docs = new List<Document>();
            try
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    Documents docsC = JsonConvert.DeserializeObject<Documents>(json);
                    docs = docsC.documents;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
            return docs;
        }
    }
}