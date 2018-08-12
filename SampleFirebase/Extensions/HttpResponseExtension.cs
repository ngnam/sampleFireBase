using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SampleFirebase.Extensions
{
    public static class HttpResponseExtension
    {
        public static string ToJsonString(this object model)
        {
            if (model is string) throw new ArgumentException("mode should not be a string");
            return JsonConvert.SerializeObject(model);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, string url, object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var data = new StringContent(content: json,
                 encoding: Encoding.UTF8,
                 mediaType: "application/json");
            return httpClient.PostAsync(url, data);
        }

        public static Task<HttpResponseMessage> GetAsJsonAsync(this HttpClient httpClient, string url)
        {
            return httpClient.GetAsync(url);
        }
    }
}
