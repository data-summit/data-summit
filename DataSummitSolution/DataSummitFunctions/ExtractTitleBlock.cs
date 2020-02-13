using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitFunctions.Models;
using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Models.Recognition;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace DataSummitFunctions
{
    public static class ExtractTitleBlock
    {
        [FunctionName("ExtractTitleBlock")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                //dynamic data = JsonConvert.DeserializeObject<List<Sentences>>(jsonContent);
                //List<Sentences> lFeatures = (List<Sentences>)data;
                ImageUpload iu = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                int Tolerance = 5;

                //Scale
                var a1 = iu.Sentences.Count(bw => bw.Words.Contains("NTS"));
                var b1 = iu.Sentences.Where(bw => bw.Words.Contains("NTS")).ToList();
                //C
                var a2 = iu.Sentences.Count(bw => bw.Words.Contains("PB"));
                var b2 = iu.Sentences.Where(bw => bw.Words.Contains("PB")).ToList();
                //Drawing Number
                var a3 = iu.Sentences.Count(bw => bw.Words.Contains("101"));
                var b3 = iu.Sentences.Where(bw => bw.Words.Contains("101")).ToList();
                //Revision
                var a4 = iu.Sentences.Count(bw => bw.Words.Contains("C1"));
                var b4 = iu.Sentences.Where(bw => bw.Words.Contains("C1")).ToList();

                foreach (ProfileAttributes pa in iu.ProfileVersion.ProfileAttributes)
                {
                    string name = pa.Name;
                    var instances = iu.Sentences.Count(b => 
                                              (b.Left > (pa.ValueX - Tolerance) && b.Left < (pa.ValueX + pa.ValueWidth) + Tolerance) &&
                                              (b.Top > (pa.ValueY - Tolerance) && b.Top < (pa.ValueY + pa.ValueHeight) + Tolerance));
                    var sentences = iu.Sentences.Where(b =>
                                              (b.Left > (pa.ValueX - Tolerance) && b.Left < (pa.ValueX + pa.ValueWidth) + Tolerance) &&
                                              (b.Top > (pa.ValueY - Tolerance) && b.Top < (pa.ValueY + pa.ValueHeight) + Tolerance)).ToList();
                    if (instances == 1)
                    {
                        pa.Value = sentences[0].Words;
                    }
                    else if (instances > 1)
                    {
                        //Combine results for now
                        pa.Value = string.Join(" ", sentences.Select(w => w.Words).ToList());
                        // Additional tests for multiple results to determine containment
                        // or whether specific words are to be split
                    }
                    else
                    { }
                }

                string jsonToReturn = ""; // JsonConvert.SerializeObject(lFeatures);

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
