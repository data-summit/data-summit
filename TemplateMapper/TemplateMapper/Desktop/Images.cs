using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.Desktop
{
    public static class Images
    {
        public static Image FromDisk(string filePath)
        {
            Image img = null;
            try
            {
                img = Bitmap.FromFile(filePath);
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return img;
        }
    }
}
