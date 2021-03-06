using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Google.Response
{
    [Serializable]
    public class BoundingBox
    {
        public List<Vertex> vertices { get; set; }

        public BoundingBox()
        { }
    }
}
