using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PDFTextAnalyticsNLP.Excel
{
    public static class App
    {
        public static Microsoft.Office.Interop.Excel.Application xlApp;
        public static List<Microsoft.Office.Interop.Excel.Worksheet> lSheets = new List<Microsoft.Office.Interop.Excel.Worksheet>();
        public static Workbook wbC;
        public static List<Worksheet> lws;
        public static List<List<ListObject>> lllo;
        public static bool Launch()
        {
            try
            {
                Console.WriteLine("Starting 'Excel'");
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = true;
                xlApp.WindowState = XlWindowState.xlMaximized;
                //xlApp.WindowState = XlWindowState.xlMinimized;
                //Console.WriteLine("Minimized 'Excel'");

                wbC = xlApp.Workbooks.Add(Type.Missing);
                lws = new List<Worksheet>();
                lllo = new List<List<ListObject>>();
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
                Terminate();
                return false;
            }
            return true;
        }

        public static void SaveDialog(string sInitialName)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel files (*.xlsx)|*.xlsx";
                sfd.FilterIndex = 0;
                sfd.RestoreDirectory = false;
                sfd.CreatePrompt = true;
                sfd.ValidateNames = true;
                sfd.Title = "Save File as...";
                if (sInitialName != "")
                {
                    sfd.FileName = sInitialName.Substring(sInitialName.LastIndexOf("\\") + 1,
                                                          sInitialName.Length - sInitialName.LastIndexOf("\\") - 1);
                }

                xlApp.DisplayAlerts = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    wbC.SaveAs(sfd.FileName.ToString(), XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, false,
                                false, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                }
                xlApp.DisplayAlerts = true;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
        }
        public static void Terminate()
        {
            try
            {
                if (wbC != null) wbC.Close(false);
                if (xlApp != null) xlApp.Quit();
                ReleaseObjects();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
        }
        public static void ReleaseObjects()
        {
            try
            {
                //Release tables
                if (null != lllo)
                {
                    foreach (List<ListObject> llo in lws)
                    {
                        foreach (ListObject lo in llo)
                        {
                            if (lo != null)
                            {
                                Marshal.ReleaseComObject(lo);
                            }
                        }
                        llo.Clear();
                    }
                    lllo.Clear();
                    lllo = null;
                }
                //Release worksheets
                if (null != lws)
                {
                    foreach (Worksheet ws in lws)
                    {
                        if (ws != null)
                        {
                            Marshal.ReleaseComObject(ws);
                        }
                    }
                    lws.Clear();
                    lws = null;
                }
                //Release workbook
                if (wbC != null)
                {
                    Marshal.ReleaseComObject(wbC);
                    wbC = null;
                }
                //Release application
                if (null != xlApp)
                {
                    Marshal.ReleaseComObject(xlApp);
                    xlApp = null;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
        }
    }
}