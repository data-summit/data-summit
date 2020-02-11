using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Models.Recognition;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace DataSummitFunctions
{
    public static class TitleBlock
    {
        [FunctionName("TitleBlock")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<List<Sentences>>(jsonContent);
                List<Sentences> lFeatures = (List<Sentences>)data;
                List<RectanglePairs> lResults = new List<RectanglePairs>();
                


                string jsonToReturn = JsonConvert.SerializeObject(lFeatures);

                return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json") };
            }
            catch (Exception ae)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                { Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json") };
            }
        }
    }
}
