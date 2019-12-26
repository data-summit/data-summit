using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.Helper
{
    public static class Sentence
    {
        public static List<OCR.Consolidated.Sentence> UpdateRatio(List<OCR.Consolidated.Sentence> sentences)
        {
            try
            {
                foreach (OCR.Consolidated.Sentence s in sentences)
                {
                    s.SlendernessRatio = s.Rectangle.Width / s.Rectangle.Height;
                }
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return sentences;
        }
    }
}
