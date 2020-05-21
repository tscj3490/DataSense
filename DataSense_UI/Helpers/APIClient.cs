using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace DataSense_UI.Helpers
{
    public class APIClient
    {
        //private static HttpClient client = new HttpClient();
        private string baseURL = Configuration.APIBaseURLPath();
        private static readonly APIClient instance = new APIClient();

        public APIClient() { }

        public static APIClient Instance
        {
            get
            {
                return instance;
            }
        }
        public async Task<HttpResponseMessage> postAsync(HttpGetObject postobj, HttpContent obj, bool isPatch = false, string runEndPoint = "")
        {
            HttpResponseMessage postResponse = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            if (runEndPoint == null || runEndPoint == "")
                client.BaseAddress = new Uri(baseURL);
            else
            {
                if (!runEndPoint.EndsWith("/"))
                    runEndPoint = runEndPoint + "/";
                client.BaseAddress = new Uri(runEndPoint);
            }
            //set the request headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", postobj.accessToken);
            if (postobj.id != null)
                postobj.endPoint = postobj.endPoint.Replace("{id}", postobj.id.ToString());

            if (isPatch == true)
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), postobj.endPoint) { Content = obj };
                postResponse = await client.SendAsync(request);
            }
            else
                postResponse = await client.PostAsync(postobj.endPoint, obj);

            return postResponse;
        }

        public async Task<HttpResponseMessage> getAsync(HttpGetObject getobj)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            //set the request headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", getobj.accessToken);
            if (getobj.id != null)
                getobj.endPoint = getobj.endPoint.Replace("{id}", getobj.id.ToString());
            if(getobj.endPoint.Contains("{datasetid}"))
                getobj.endPoint = getobj.endPoint.Replace("{datasetid}", getobj.id.ToString());
            if (getobj.endPoint.Contains("{datasetindexid}"))
                getobj.endPoint = getobj.endPoint.Replace("{datasetindexid}", getobj.id.ToString());
            if (getobj.endPoint.Contains("{indexrunid}"))
                getobj.endPoint = getobj.endPoint.Replace("{indexrunid}", getobj.id.ToString());

            HttpResponseMessage profileResponse = await client.GetAsync(getobj.endPoint);
            return profileResponse;
        }

        public async Task<HttpResponseMessage> deleteAsync(HttpGetObject getobj)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            //set the request headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", getobj.accessToken);
            if (getobj.id != null)
                getobj.endPoint = getobj.endPoint.Replace("{id}", getobj.id.ToString());

            HttpResponseMessage profileResponse = await client.DeleteAsync(getobj.endPoint);
            return profileResponse;
        }

        public string getSync(HttpGetObject getobj)
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("Authorization", getobj.accessToken);
            string endpoint = baseURL + "/" + getobj.endPoint;
            string reply = client.DownloadString(endpoint);
            return reply;
        }

        public ByteArrayContent convertToContent(Object obj)
        {
            var cont = JsonConvert.SerializeObject(obj);
            var buffer = System.Text.Encoding.UTF8.GetBytes(cont);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
    public class HttpGetObject
    {
        public string accessToken { get; set; }
        public string endPoint { get; set; }
        public string id { get; set; }
    }
}
