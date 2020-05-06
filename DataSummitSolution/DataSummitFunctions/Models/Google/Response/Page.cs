using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Page
    {
        public Property property { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<Block> blocks { get; set; }

        public Page()
        { }
    }
}
