using System;

namespace AzureFunctions.Models.Amazon
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
    
