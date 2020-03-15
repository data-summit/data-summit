using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF2Word.Word
{
    public static class App
    {
        public static Microsoft.Office.Interop.Word.Application wdApp;
        public static Document cDoc;
        public static Selection cSel;

        public static bool Launch()
        {
            try
            {
                Console.WriteLine("Starting 'Word'");
                wdApp = new Microsoft.Office.Interop.Word.Application
                {
                    //Visible = true,
                    //WindowState = WdWindowState.wdWindowStateMaximize
                    WindowState = WdWindowState.wdWindowStateMinimize
                };
                Console.WriteLine("Word started");
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
                Terminate();
                return false;
            }
            return true;
        }

        public static void OpenPDFDoc(string fileName)
        {
            try
            {
                FileConverter fcPDF = wdApp.FileConverters[4];

                cDoc = wdApp.Documents.Open(FileName: fileName, ConfirmConversions: true, ReadOnly: false, AddToRecentFiles: false,
                    PasswordDocument: "", PasswordTemplate: "", Revert: false, WritePasswordDocument: "", WritePasswordTemplate: "",
                    Format: fcPDF.OpenFormat, Encoding: Type.Missing, Visible: Type.Missing, OpenAndRepair: Type.Missing, DocumentDirection: Type.Missing, 
                    NoEncodingDialog: Type.Missing, XMLTransform: Type.Missing);
                cSel = wdApp.Selection;
                Console.WriteLine("Opened document: " + fileName.Substring(fileName.LastIndexOf(@"\") + 1, fileName.Length - fileName.LastIndexOf(@"\") - 1));
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
                Terminate();
            }
        }

        public static void SaveDialog(string sInitialName)
        {
            try
            {
                string sInitial = sInitialName.Substring(0, sInitialName.LastIndexOf(@"\"));

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Word files (*.docx)|*.docx",
                    FilterIndex = 0,
                    RestoreDirectory = false,
                    CreatePrompt = true,
                    ValidateNames = true,
                    Title = "Save File as...",
                    InitialDirectory = sInitial
                };
                if (sInitialName != "")
                {
                    sfd.FileName = sInitialName.Substring(sInitialName.LastIndexOf("\\") + 1,
                                                          sInitialName.Length - sInitialName.LastIndexOf("\\") - 1);
                }

                wdApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    cDoc.SaveAs2(sfd.FileName.ToString(), WdSaveFormat.wdFormatXMLDocument, Type.Missing, Type.Missing,
                                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                wdApp.DisplayAlerts = WdAlertLevel.wdAlertsAll;
                Console.WriteLine("Saved document: " + sfd.FileName.Substring(sfd.FileName.LastIndexOf(@"\") + 1, sfd.FileName.Length - sfd.FileName.LastIndexOf(@"\") - 1));

                Export.Images(sInitial);
                Export.Tables(sInitial);
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
                if (cDoc != null) cDoc.Close(false);
                if (wdApp != null) wdApp.Quit();
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
                //Release document
                if (cDoc != null)
                {
                    Marshal.ReleaseComObject(cDoc);
                    cDoc = null;
                }
                //Release application
                if (null != wdApp)
                {
                    Marshal.ReleaseComObject(wdApp);
                    wdApp = null;
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
