using System;

namespace AzureFunctions.Models.Google.Response
{
    [Serializable]
    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }

        public Vertex()
        { }
    }
}
