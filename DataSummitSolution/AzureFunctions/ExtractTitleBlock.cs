using AzureFunctions.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AzureFunctions
{
    public static class ExtractTitleBlock
    {
        [FunctionName("ExtractTitleBlock")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload iu = (ImageUpload)data;
                //ImageUpload img = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                int Tolerance = 3;

                foreach (ProfileAttributes pa in iu.ProfileAttributes)
                {
                    string name = pa.Name;
                    var instances = iu.Sentences.Count(b =>
                                              (b.Left > (pa.ValueX - Tolerance) && b.Left < (pa.ValueX + pa.ValueWidth) + Tolerance) &&
                                              (b.Top > (pa.ValueY - Tolerance) && b.Top < (pa.ValueY + pa.ValueHeight) + Tolerance));

                    if (instances > 0)
                    {
                        var sentences = iu.Sentences.Where(b =>
                                              (b.Left > (pa.ValueX - Tolerance) && b.Left < (pa.ValueX + pa.ValueWidth) + Tolerance) &&
                                              (b.Top > (pa.ValueY - Tolerance) && b.Top < (pa.ValueY + pa.ValueHeight) + Tolerance)).ToList();
                        if (pa.Properties == null) pa.Properties = new List<Properties>();
                        if (instances == 1)
                        {
                            pa.Value = sentences[0].Words;
                            Properties p = new Properties();
                            p.SentenceId = sentences[0].SentenceId;
                            p.ProfileAttributeId = pa.ProfileAttributeId;
                            pa.Properties.Add(p);
                        }
                        else if (instances > 1)
                        {
                            //Combine results for now
                            pa.Value = string.Join(" ", sentences.Select(w => w.Words).ToList()).Trim();
                            // Additional tests for multiple results to determine containment
                            // or whether specific words are to be split
                            foreach (var s in sentences)
                            {
                                Properties p = new Properties();
                                p.SentenceId = s.SentenceId;
                                p.ProfileAttributeId = pa.ProfileAttributeId;
                                pa.Properties.Add(p);
                            }
                        }
                        else
                        { }
                    }
                }

                string jsonToReturn = JsonConvert.SerializeObject(iu);

                return new OkObjectResult(jsonToReturn);
            }
            catch (Exception ae)
            {
                //return error generated within function code
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ae));
            }
        }
    }
}