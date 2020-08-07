using Diagnostics.Logger;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Office.Outlook
{
    public static class PST
    {
        private static string fol = @"C:\Users\TomJames_zyl8law\OneDrive\Employers Work\HC\HC QS Outlook";
        public static List<string> stores = new List<string>();
        public static void Extract()
        {
            try
            {
                List<string> files = Directory.EnumerateFiles(fol).Where(f =>
                            f.Substring(f.LastIndexOf(".") + 1, 3).ToUpper() == "PST").ToList();

                foreach (var f in files)
                {
                    //Verify that store is not already attached as a data file
                    if (StoreExists(f) == false)
                    {
                        App.oNameSpace.AddStoreEx(f, OlStoreType.olStoreUnicode);
                        Thread.Sleep(1000);
                    }

                    string file = f.Substring(f.LastIndexOf("\\") + 2, f.Length - f.LastIndexOf("\\") - 2);

                    for (int i = 0; i < App.olApp.Session.Stores.Count; i++)
                    {
                        Store store = null;
                        try
                        {
                            store = App.olApp.Session.Stores[i];
                        }
                        catch (System.Exception)
                        { }

                        if (store != null)
                        {
                            if (store.FilePath == null)
                            { stores.Add(""); }
                            else
                            {
                                stores.Add(store.FilePath.Substring(
                                           store.FilePath.LastIndexOf("\\"),
                                           store.FilePath.Length - store.FilePath.LastIndexOf("\\")));
                            }

                            if (store.FilePath == f)
                            {
                                Folder sf = (Folder)store.GetRootFolder();
                                //Iterate sub folders and extract e-mails
                                Folders(sf, file);

                                //Remove store after iterating contents
                                App.oNameSpace.RemoveStore(sf);

                                //Limit the total output
                                if (Excel.App.row > 1048500)
                                {
                                    Excel.App.xlApp.StatusBar = "";
                                    Excel.App.xlApp.ScreenUpdating = true;
                                    return;
                                }
                            }
                        }
                        else
                        { }
                    }
                }
                //Excel.App.Close();
            }
            catch (System.Exception ae)
            {
                Log.WriteLine(ae.StackTrace);
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }

        private static bool StoreExists(string storePath)
        {
            try
            {
                for (int i = 0; i < App.olApp.Session.Stores.Count; i++)
                {
                    Store store = null;
                    try
                    {
                        store = App.olApp.Session.Stores[i];
                    }
                    catch (System.Exception)
                    { }

                    if (store != null && store.FilePath == storePath)
                    { return true; }
                }
            }
            catch (System.Exception ae)
            {
                Log.WriteLine(ae.StackTrace);
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
            return false;
        }

        private static void Folders(Folder f, string file)
        {
            try
            {
                if (f.Items.Count > 0 || f.Folders.Count > 0)
                {
                    if (Excel.App.xlApp == null)
                    {
                        Excel.App.Create();
                    }
                }

                //Extract e-mail items at current folder level
                foreach (var i in f.Items)
                {
                    if (i is MailItem)
                    {
                        MailItem ml = (MailItem)i;
                        if (ml.SenderEmailType == "EX")
                        {
                            ExchangeUser exU = ml.Sender.GetExchangeUser();
                            if (exU != null && exU.PrimarySmtpAddress != null)
                            {
                                Excel.App.wsData.Cells[Excel.App.row, 1].Formula = exU.PrimarySmtpAddress.ToString();
                            }
                            else
                            {
                                if (ml.Sender != null)
                                {
                                    Excel.App.wsData.Cells[Excel.App.row, 1].Formula =
                                        ml.Sender.Name.ToString().Substring(
                                            ml.Sender.Name.ToString().LastIndexOf("CN=") + 3,
                                            ml.Sender.Name.ToString().Length - ml.Sender.Name.ToString().LastIndexOf("CN=") - 3);
                                }
                            }
                        }
                        else
                        {
                            if (ml.Sender != null) Excel.App.wsData.Cells[Excel.App.row, 1].Formula = ml.Sender.Name.ToString();
                        }

                        List<Recipient> lRcp = new List<Recipient>();
                        foreach (var rcp in ml.Recipients)
                        { lRcp.Add((Recipient)rcp); }
                        lRcp.RemoveAll(r => r.Name == null && r.Name == "");
                        if (lRcp.Count > 0)
                        { Excel.App.wsData.Cells[Excel.App.row, 2].Formula = lRcp[0].Name.ToString(); }
                        else
                        { }

                        Excel.App.wsData.Cells[Excel.App.row, 3].Formula = ml.SentOn.ToString();
                        if (ml.Subject != null) Excel.App.wsData.Cells[Excel.App.row, 4].Formula = ml.Subject.ToString();
                        Excel.App.wsData.Cells[Excel.App.row, 5].Formula = ml.Attachments.Count;
                        Excel.App.wsData.Cells[Excel.App.row, 6].Formula = DateTime.Now.ToString();
                        Excel.App.wsData.Cells[Excel.App.row, 7].Formula = DateTime.Now.Subtract(Excel.App.start).Ticks; 
                        Excel.App.wsData.Cells[Excel.App.row, 8].Formula = file;

                        List<string> fols = f.FullFolderPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        for (int j = 0; j < fols.Count; j++)
                        {
                            Excel.App.wsData.Cells[Excel.App.row, 9 + j].Formula = fols[j];
                            if (j > Excel.App.folderNr)
                            {
                                Excel.App.xlApp.ScreenUpdating = true;
                                Excel.App.wsData.Cells[1, 9 + j].Formula = "Folder " + (1 + j).ToString();
                                Excel.App.folderNr = (short)j;
                                Excel.App.xlApp.ScreenUpdating = false;
                            }
                        }
                        Excel.App.row += 1;
                        Excel.App.xlApp.StatusBar = Excel.App.row.ToString() + " rows added";
                    }
                }

                //Recursively loop into sub-folders
                if (f.Folders.Count > 0)
                {
                    foreach (var sf in f.Folders)
                    {
                        Folders((Folder)sf, file);
                    }
                }
            }
            catch (System.Exception ae)
            {
                Log.WriteLine(ae.StackTrace);
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }
    }
}