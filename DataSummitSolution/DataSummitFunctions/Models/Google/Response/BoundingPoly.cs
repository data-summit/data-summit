using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class BoundingPoly
    {
        public List<Vertex> vertices { get; set; }

        public BoundingPoly()
        { }
    }
}
