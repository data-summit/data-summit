using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFTextAnalyticsNLP
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Form.Import i = new Form.Import();
            Form.Export e = new Form.Export();

            string sInit = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\Test PDFs\Document OCR Extraction\Sample";
            string fInput = i.OpenPDF(sInit);
            fInput = fInput.Replace(".pdf", ".xlsx");

            Excel.App.Launch();
            string fileName = @"C:\Users\TomJames_zyl8law\Data Summit Ltd\Data Summit Hub - Documents\20-Marketing\Document to Text\02-Form & Table.json";
            List<Form.Document> docs = i.ReadFile(fileName);
            e.CreateTables(docs);

            Excel.App.SaveDialog(fInput);
            Excel.App.Terminate();
        }
    }
}
