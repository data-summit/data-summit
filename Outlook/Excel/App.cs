using Diagnostics.Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Office.Excel
{
    public static class App
    {
        public static Application xlApp = null;
        private static Workbook wbC = null;
        public static Worksheet wsData = null;
        public static int row = 2;
        public static short folderNr = 0;
        public static DateTime start;
        //[STAThread]
        public static void Create()
        {
            try
            {
                string template = @"C:\Users\TomJames_zyl8law\Lightos\Lightos Hub - Documents\01-Clients\BYP Associates\2020-Epsom Residential\BoQ Template.xlsm";

                if (File.Exists(template))
                {
                    xlApp = new Application
                    {
                        Visible = true,
                        WindowState = XlWindowState.xlMaximized
                    };
                    wbC = xlApp.Workbooks.Add();

                    wsData = xlApp.ActiveSheet;
                    wsData.Name = "Data";

                    wsData.Cells[1, 1].Formula = "Sender";
                    wsData.Cells[1, 2].Formula = "To";
                    wsData.Cells[1, 3].Formula = "Sent Date";
                    wsData.Cells[1, 4].Formula = "Subject";
                    wsData.Cells[1, 5].Formula = "Attachments";
                    wsData.Cells[1, 6].Formula = "Timestamp";
                    wsData.Cells[1, 7].Formula = "Duration";
                    wsData.Cells[1, 8].Formula = "File";
                    wsData.Cells[1, 9].Formula = "Folder 1";

                    start = DateTime.Now;
                    xlApp.ScreenUpdating = false;

                    //    Worksheet wsTOS = wbC.Worksheets["Take Off Schedule"];
                    //    Worksheet wsErr = wbC.Worksheets["Errors"];
                    //    Worksheet wsTemplate = wbC.Worksheets["Template"];

                    //    wsTOS.Activate();
                    //    ListObject loBoQ = wsData.ListObjects["Data"];
                    //    ListObject loTOS = wsTOS.ListObjects["TakeOffSchedule"];

                    //    //Clear previous 'Take Off Schedule' data
                    //    if (loTOS.ListRows.Count > 0)
                    //    {
                    //        loTOS.DataBodyRange.Delete();
                    //    }


                }
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.StackTrace);
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }
        public static void Close()
        {
            try
            {
                //    path = @"C:\Users\TomJames_zyl8law\Lightos\Lightos Hub - Documents\01-Clients\BYP Associates\2020-Epsom Residential\Outputs\Sample Bill";
                //    if (File.Exists(path + ".xlsm"))
                //    {
                //        File.Delete(path + ".xlsm");
                //    }

                //    wbC.SaveAs(path + ".xlsm", XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
                wbC.Close();
                xlApp.Quit();
                Marshal.ReleaseComObject(wbC);
                Marshal.ReleaseComObject(xlApp);

            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.StackTrace);
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }
    }
}