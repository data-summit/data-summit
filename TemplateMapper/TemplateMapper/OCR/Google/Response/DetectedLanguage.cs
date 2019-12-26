using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class DetectedLanguage
    {
        //public LanguageCode languageCode { get; set; }
        public string languageCode { get; set; }

        public DetectedLanguage()
        { }

        public enum LanguageCode
        {
            af = 1,
            ar = 2,
            As = 3,
            az = 4,
            be = 5,
            bn = 6,
            bg = 7,
            ca = 8,
            zh = 9,
            hr = 10,
            cs = 11,
            da = 12,
            nl = 13,
            en = 14,
            et = 15,
            fil = 16,
            tl = 17,
            fi = 18,
            fr = 19,
            de = 20,
            el = 21,
            he = 22,
            iw = 23,
            hi = 24,
            hu = 25,
            Is = 26,
            id = 27,
            it = 28,
            ja = 29,
            kk = 30,
            ko = 31,
            ky = 32,
            lv = 33,
            lt = 34,
            mk = 35,
            mr = 36,
            mn = 37,
            ne = 38,
            no = 39,
            ps = 40,
            fa = 41,
            pl = 42,
            pt = 43,
            ro = 44,
            ru = 45,
            sa = 46,
            sr = 47,
            sk = 48,
            sl = 49,
            es = 50,
            sv = 51,
            ta = 52,
            th = 53,
            tr = 54,
            uk = 55,
            ur = 56,
            uz = 57,
            vi = 58,
            //Other languages found but not listed on: https://cloud.google.com/vision/docs/languages
            co = 59,
            jv = 60

        }
    }
}
