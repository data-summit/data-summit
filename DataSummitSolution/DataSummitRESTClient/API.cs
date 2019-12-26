using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DataSummitREST
{
    public class API
    {
        private static readonly HttpClient client = new HttpClient();

        public static HttpResponseMessage ProcessCall(Uri uri, string payload)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

            HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);

            var stringTask = client.PostAsync(uri, httpContent);
            return stringTask.Result;
        }
    }
}
