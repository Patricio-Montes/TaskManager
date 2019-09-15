using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiClient
{
    internal static class BaseAddress
    {
        public static HttpClient WebApiClient = new HttpClient();

        static BaseAddress()
        {
            WebApiClient.BaseAddress = new Uri("https://localhost:44349/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            // Formato para los request de la API. Se modifica el tratamiento de formato XML a Json. 
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}