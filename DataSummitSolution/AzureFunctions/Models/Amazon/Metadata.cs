using System;

namespace AzureFunctions.Models.Amazon
{
    [Serializable]
    public class Metadata
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Metadata()
        { }
    }
}
