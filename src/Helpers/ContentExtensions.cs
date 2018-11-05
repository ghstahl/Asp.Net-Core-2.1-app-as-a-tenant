using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Helpers
{
    public static class ContentExtensions
    {
        public static async Task<HttpContent> ToJsonContentAsync(this Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = await reader.ReadToEndAsync();
                return new StringContent(json, Encoding.UTF8, "application/json");
            }
        }
        public static HttpContent ToJsonContent<T>(this T obj) where T:class
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        public static  HttpContent ToFormUrlEncodedContent(this IFormCollection form) 
        {
            var query = from item in form
                        let c = new KeyValuePair<string, string>(item.Key, item.Value)
                select c;
            return new FormUrlEncodedContent(query);
        }
    }
}


