using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Amazon
{
    [Serializable]
    public class Type
    {
        //public ValueType Value { get; set; }
        public string Value { get; set; }

        public Type()
        { }

        public enum ValueType
        {
            LINE = 1,
            WORD = 2
        }
    }
}
    
