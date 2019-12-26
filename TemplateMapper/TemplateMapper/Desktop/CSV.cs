using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TemplateMapper.Desktop
{
    public static class CSV
    {
        public static ImageUpload FromDisk(string filePath)
        {
            ImageUpload imgS = new ImageUpload();
            try
            {
                //Read data from csv file within blob
                var reader = new StreamReader(File.OpenRead(filePath + "\\File data.csv"));
                List<string> searchList = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    searchList.Add(line);
                }
                reader.Close();
                reader = null;

                if (searchList.Count > 0)
                {
                    char[] delimiterChars = { ',' };
                    long l = long.MinValue;
                    foreach (string s in searchList)
                    {
                        string[] words = s.Split(delimiterChars);
                        string strPathName = filePath + "\\" + words[0];

                        //if (File.Exists(strPathName) == true)
                        //{
                        //    ImageGrid ig = new ImageGrid
                        //    {
                        //        //File name
                        //        Name = words[0],
                        //        Path = filePath
                        //    };

                        //    //File horizontal location index
                        //    string ss = words[0].Substring(words[0].IndexOf("_") + 1, words[0].IndexOf("-") - words[0].IndexOf("_") - 1);
                        //    long.TryParse(ss, out l);
                        //    ss = "";
                        //    if (l > int.MinValue)
                        //    {
                        //        ig.iloc = (int)l;
                        //        l = long.MinValue;
                        //    }

                        //    //File vertical location index
                        //    ss = words[0].Substring(words[0].IndexOf("-") + 1, words[0].IndexOf(".") - words[0].IndexOf("-") - 1);
                        //    long.TryParse(ss, out l);

                        //    if (l > int.MinValue)
                        //    {
                        //        ig.jloc = (int)l;
                        //        l = long.MinValue;
                        //    }

                        //    //i (vertical) pixel start
                        //    long.TryParse(words[1], out l);
                        //    if (l > long.MinValue)
                        //    {
                        //        ig.WidthStart = (int)l;
                        //        l = long.MinValue;
                        //    }

                        //    //j (vertical) pixel start
                        //    long.TryParse(words[2], out l);
                        //    if (l > long.MinValue)
                        //    {
                        //        ig.HeightStart = (int)l;
                        //        l = long.MinValue;
                        //    }

                        //    //i (vertical) pixel end
                        //    long.TryParse(words[3], out l);
                        //    if (l > long.MinValue)
                        //    {
                        //        ig.iPixEnd = l;
                        //        l = long.MinValue;
                        //    }

                        //    //j (vertical) pixel end
                        //    long.TryParse(words[4], out l);
                        //    if (l > long.MinValue)
                        //    {
                        //        ig.jPixEnd = l;
                        //        l = long.MinValue;
                        //    }

                        //    //Define image type
                        //    if (words[5] == "Normal" || words[5] == "Overlap")
                        //    {
                        //        if (words[5] == "Normal")
                        //        { ig.Type = ImageGrid.ImageType.Normal; }
                        //        if (words[5] == "Overlap")
                        //        { ig.Type = ImageGrid.ImageType.Overlap; }
                        //    }

                        //    //Get image data
                        //    ig.Image = Image.FromFile(strPathName);

                        //    //Add to collection
                        //    Drawings.Add(ig);
                        //}
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.Message.ToString();
            }
            return imgS;
        }
    }
}
