using Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PDF2Word.Word
{
    public static class Export
    {
        public static void Images(string DirectoryPath)
        {
            try
            {
                for (int i = 1; i <= App.cDoc.InlineShapes.Count; i++)
                {
                    InlineShape cShape = App.cDoc.InlineShapes[i];
                    cShape.Select();
                    App.wdApp.Selection.Copy();

                    //PictureFormat pf = cShape.PictureFormat;
                    
                    // Check data is in the clipboard
                    if (Clipboard.GetDataObject() != null)
                    {
                        var data = Clipboard.GetDataObject();
                        // Check if the data conforms to a bitmap format
                        if (data != null)
                        {
                            if (Clipboard.ContainsImage() == true)
                            { 
                                // Fetch the image and convert it to a Bitmap
                                var image = (Image)data.GetData(DataFormats.Bitmap, true);
                                var currentBitmap = new Bitmap(image);

                                // Save the bitmap to a file
                                currentBitmap.Save(DirectoryPath + @"\Image " + i.ToString("000") + ".png",
                                    System.Drawing.Imaging.ImageFormat.Png);
                                Console.WriteLine("Image " + i.ToString("000") + " created");
                            }
                            
                        }
                        Clipboard.Clear();
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
        }
        public static void Tables(string DirectoryPath)
        {
            try
            {
                for (int i = 1; i <= App.cDoc.Tables.Count; i++)
                {
                    Table t = App.cDoc.Tables[i];
                    using (StreamWriter writer = File.CreateText(DirectoryPath + @"\Table " + i.ToString("000") + ".csv"))
                    {
                        foreach (Row r in t.Rows)
                        {
                            string row = "";
                            foreach (Cell c in r.Cells)
                            {
                                string cell = c.Range.Text;
                                cell = cell.Replace("\r\a", "").Trim();
                                row = row + cell + ",";
                            }
                            row = row.Substring(0, row.Length - 1).Trim();
                            writer.WriteLine(row);
                        }
                    }
                    Console.WriteLine("Table " + i.ToString("000") + " created");
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.Message.ToString();
            }
        }

        public enum DatFormats
        {
            Bitmap = 1,
            CommaSeparatedValue = 2,
            Dib = 3,
            Dif = 4,
            EnhancedMetafile = 5,
            FileDrop = 6,
            Html = 7,
            Locale = 8,
            MetafilePicture = 9,
            OemText = 10,
            Palette = 11,
            PenData = 12,
            Riff = 13,
            Rtf = 14,
            Serializable = 15,
            StringFormat = 16,
            SymbolicLink = 17,
            Text = 18,
            Tiff = 19,
            UnicodeText = 20,
            WaveAudio = 21,
            Xaml = 22,
            XamlPackage = 23
        }
    }
}
