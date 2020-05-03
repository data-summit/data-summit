using System.Collections.Generic;

namespace DataSummitFunctions.Models.Google.Response
{
    public class Cloud
    {
        public List<Responses> responses { get; set; }

        public Cloud()
        {
            responses = new List<Responses>();
        }
    }
}
