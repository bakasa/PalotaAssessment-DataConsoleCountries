using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace PalotaInterviewCS
{
    public static class HttpRestApiHelper
    {
        private static HttpClient _client = new HttpClient();

        public static string GetAsync(string url)
        {
            var response = _client.GetStringAsync(url).Result;
            
            // some checking could be done here before we return

            return response;
        }

        public static void PostAsync<T>(string url, byte[] data)
        {
            var result = _client.PostAsync(url, new ByteArrayContent(data)).Result;
        }

        public static void PutAsync<T>(string url, byte[] data)
        {
            var result = _client.PutAsync(url, new ByteArrayContent(data)).Result;
        }

        public static void DeleteAsync(string url)
        {
            var result = _client.DeleteAsync(url).Result;
        }
    }
}