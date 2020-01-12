﻿namespace DataImportService
{
    using System.Net.Http;

    public static class HttpClientExtensions
    {
        public static HttpClient SetupHeaders(this HttpClient client)
        {
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:44.0) Gecko/20100101 Firefox/44.0");
            client.DefaultRequestHeaders.Add("Accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");

            return client;
        }
    }
}