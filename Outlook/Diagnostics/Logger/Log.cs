using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostics.Logger
{
    public static class Log
    {
        public static List<Event> Events = new List<Event>();
        public static StreamWriter TextFile;
        public static void Create()
        {
            if (File.Exists(Environment.CurrentDirectory.ToString() + "\\ErrorLog.txt") == true)
            {
                File.Delete(Environment.CurrentDirectory.ToString() + "\\ErrorLog.txt");
            }
            try
            {
                TextFile = new StreamWriter(Environment.CurrentDirectory.ToString() + "\\ErrorLog.txt");
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.Message);
            }
        }
        public static void WriteLine(String InputLine)
        {
            //Text file needs to be initiated prior to writing
            TextFile.WriteLine(InputLine);
        }
        public static void Close()
        {
            TextFile.Close();
        }
        public static async Task WriteToFileAsync(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);

                using (StreamWriter sw = new StreamWriter(path))
                {
                    await sw.WriteLineAsync("Id,Message,Source,Start,End,Duration");
                    foreach (Event e in Events)
                    {
                        try
                        {
                            await sw.WriteLineAsync(e.Id.ToString() + "," + e.Message.ToString() + "," + e.SourceType.ToString() + "," +
                                e.Start.ToString() + "," + e.End.ToString() + "," + e.Duration.ToString("mm\\:ss\\.fff", null));
                        }
                        catch (Exception ex)
                        { string s = ex.Message.ToString(); }
                    }
                    sw.Close();
                }
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }
    }
    public static class DataDump
    {
        public static StreamWriter TextFile;
        public static void Create()
        {
            if (File.Exists(Environment.CurrentDirectory.ToString() + "\\DataDump.csv") == true)
            {
                File.Delete(Environment.CurrentDirectory.ToString() + "\\DataDump.csv");
            }
            try
            {
                TextFile = new StreamWriter(Environment.CurrentDirectory.ToString() + "\\DataDump.csv");
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.Message);
            }
        }
        public static void WriteData(DataTable dtDump)
        {
            if (TextFile == null)
            {
                Create();
            }
            if (TextFile != null)
            {
                StringBuilder sb = new StringBuilder();
                string[] columnNames = dtDump.Columns.Cast<DataColumn>().
                                      Select(column => column.ColumnName).
                                      ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in dtDump.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                    ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }
                TextFile.Write(sb.ToString());
                Close();
            }
        }
        public static void Close()
        {
            TextFile.Close();
        }
    }
}
