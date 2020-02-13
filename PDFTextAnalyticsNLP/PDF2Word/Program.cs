using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF2Word
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            PDFTextAnalyticsNLP.Form.Import i = new PDFTextAnalyticsNLP.Form.Import();

            string sInit = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Document OCR Extraction\Sample";
            string fInput = i.OpenPDF(sInit);

            Word.App.Launch();
            Word.App.OpenPDFDoc(fInput);

            fInput = fInput.Replace(".pdf", ".docx");
            Word.App.SaveDialog(fInput);
            Word.App.Terminate();
        }
    }
}
