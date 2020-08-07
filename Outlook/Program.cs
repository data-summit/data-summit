using Diagnostics.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Log.Create();

            Outlook.App.Launch();
            Outlook.PST.Extract();
            Outlook.App.Exit();
        }
    }
}
