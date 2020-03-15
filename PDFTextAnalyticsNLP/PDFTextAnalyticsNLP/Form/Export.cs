using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDFTextAnalyticsNLP.Form
{
    public class Export
    {
        public bool CreateTables(List<Document> docs)
        {
            try
            {
                int iTable = 1; int iColMin = 1; int iColMax = 2;
                int iRowMax = 0;
                foreach (Form.Document doc in docs)
                {
                    Worksheet ws = null;
                    if (docs.IndexOf(doc) == 0)
                    {
                        ws = (Worksheet)Excel.App.wbC.Worksheets["Sheet1"];
                    }
                    else
                    {
                        ws = (Worksheet)Excel.App.wbC.Worksheets.Add(Type.Missing, Excel.App.wbC.Worksheets[
                                                                        Excel.App.wbC.Worksheets.Count - 1],
                                                                        1, XlSheetType.xlWorksheet);
                    }
                    List<List<ListObject>> llo = new List<List<ListObject>>();
                    foreach (Form.Page p in doc.pages)
                    {
                        //Create table for 'Key Pair Values'
                        ws.Range[ws.Cells[1, iColMin], ws.Cells[1, iColMin]].Formula = "Name";
                        ws.Range[ws.Cells[1, 2], ws.Cells[1, 2]].Formula = "Value";
                        for (int i = 0; i < p.keyValuePairs.Count; i++)
                        {
                            ws.Range[ws.Cells[i + 2, iColMin], ws.Cells[i + 2, iColMin]].Formula = p.keyValuePairs[i].key[0].text;
                            ws.Range[ws.Cells[i + 2, 2], ws.Cells[i + 2, 2]].Formula = p.keyValuePairs[i].value[0].text;
                        }
                        Range rLoc = ws.Range[ws.Cells[1, iColMin], ws.Cells[1, iColMin]];
                        Range rDat = ws.Range[ws.Cells[1, iColMin], ws.Cells[p.keyValuePairs.Count + 1, iColMin + 1]];
                        ListObject lo = ws.ListObjects.AddEx(XlListObjectSourceType.xlSrcRange, rDat, false,
                                                XlYesNoGuess.xlYes, rLoc, "TableStyleLight1");
                        lo.Name = "KeyPairs" + iTable.ToString("0000");
                        llo.Add(new List<ListObject> { lo });
                        Console.WriteLine("Create keyPr " + iTable.ToString() + " from page " + p.ToString());

                        iColMin = iColMax + 2;
                        List<ListObject> loTabs = new List<ListObject>();
                        for (int iTab = 0; iTab < p.tables.Count; iTab++)
                        {
                            Form.Table t = p.tables[iTab];
                            for (int j = 0; j < t.columns.Count; j++)
                            {
                                Form.Column c = t.columns[j];
                                List<Row> rows = new List<Row>();
                                for (int h = 0; h < c.header.Count; h++)
                                {
                                    ws.Range[ws.Cells[1, iColMin + h + j], ws.Cells[1, iColMin + h + j]].Formula = c.header[h].text;
                                    foreach (object o in c.entries)
                                    {
                                        var ro = JsonConvert.DeserializeObject<List<Row>>(o.ToString());
                                        if (ro.Count == 1)
                                        { rows.Add(ro[0]); }
                                        else
                                        {
                                            Row r = new Row();
                                            r.boundingBox = ro[0].boundingBox;
                                            r.text = string.Join("", ro.Select(tx => tx.text));
                                            rows.Add(r);
                                        }
                                    }
                                    for (int i = 0; i < rows.Count; i++)
                                    {
                                        Row r = rows[i];
                                        if (r.text != "__emptycell__")
                                        { ws.Range[ws.Cells[i + 2, iColMin + h + j], ws.Cells[i + 2, iColMin + h + j]].Formula = r.text; }
                                        else
                                        { }
                                    }
                                }
                                //Get last row
                                if (rows.Count + 1 > iRowMax) iRowMax = rows.Count +1;

                                //Get last column
                                if (t.columns.Count + iColMin - 1 > iColMax)
                                { iColMax = t.columns.Count + iColMin - 1; }
                                
                            }
                            rLoc = ws.Range[ws.Cells[1, iColMin], ws.Cells[1, iColMin]];
                            rDat = ws.Range[ws.Cells[1, iColMin], ws.Cells[iRowMax, iColMax]];
                            lo = ws.ListObjects.AddEx(XlListObjectSourceType.xlSrcRange, rDat, false,
                                            XlYesNoGuess.xlYes, rLoc, "TableStyleLight6");
                            lo.Name = "Table" + iTable.ToString("0000");
                            loTabs.Add(lo);
                            iTable = iTable + 1;

                            Console.WriteLine("Create table " + iTable.ToString() + " from page " + p.ToString());
                            iColMin = iColMin + 2;
                        }
                        llo.Add(loTabs);
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
                return false;
            }
            return true;
        }
    }
}
