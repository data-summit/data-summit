using Diagnostics.Logger;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Office.Outlook
{
    public static class App
    {
        public static Application olApp = null;
        public static NameSpace oNameSpace = null;
        public static MAPIFolder oFolder = null;

        [STAThread]
        public static void Launch()
        {
            try
            {
                //Launches new application
                olApp = new Application();

                //Get Namespace
                oNameSpace = olApp.GetNamespace("MAPI"); 
                //oNameSpace.Logon(null, null, true, true);
                oNameSpace.Logon("Outlook", null, true, true);

                //Displays application
                oFolder = olApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                oFolder.Display();

                //Pause to let application load correctly
                Thread.Sleep(2000);
            }
            catch (System.Exception ae)
            {
                Log.WriteLine(ae.StackTrace);
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }

        public static void Exit()
        {
            try
            {
                if (olApp != null)
                {
                    olApp.Quit();
                    Marshal.ReleaseComObject(olApp);
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