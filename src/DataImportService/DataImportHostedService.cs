namespace DataImportService
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Parsers;

    public class DataImportHostedService : IHostedService
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(HelloWorld, null, 0, 10000);
            return Task.CompletedTask;
        }

        void HelloWorld(object state)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var random = new Random(DateTime.Now.Millisecond);

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                CookieContainer = cookieContainer,
                UseCookies = true
            };

            var client = new HttpClient(handler);
            client.SetupHeaders();

            var nameValueCollection = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("login", "Tor_Oakenshield"),
                new KeyValuePair<string, string>("pass", "forza1905juventus"),
                new KeyValuePair<string, string>("pliv", GetPliv(client)),
                new KeyValuePair<string, string>("lreseted", "1"),
                new KeyValuePair<string, string>("LOGIN_redirect", "1"),
                new KeyValuePair<string, string>("x", random.Next(20, 99).ToString()),
                new KeyValuePair<string, string>("y", random.Next(20, 99).ToString())
            };

            var response = client.PostAsync(HwmUris.Login, new FormUrlEncodedContent(nameValueCollection)).Result;
            var httpResponseHeaders = response.Headers.ToString();
            var cookies = GetCookies(httpResponseHeaders);
            cookieContainer.Add(HwmUris.BasePage, new Cookie("PHPSESSID", cookies.SessionId));
            cookieContainer.Add(HwmUris.BasePage, new Cookie("pl_id", cookies.PlayerId));

            var parseTransferProtocol = new ParseTransferProtocol();
            parseTransferProtocol.Execute(client);
        }

        private static string GetPliv(HttpClient client)
        {
            var html = client.GetAsync("https://www.heroeswm.ru").Result.Content.ReadAsStringAsync().Result;

            var regex = new Regex("name=pliv value=(\\d+)");
            var match = regex.Match(html);
            return match.Groups[1].ToString();
        }

        private static HwmCookies GetCookies(string headers)
        {
            var regex1 = new Regex("PHPSESSID=((?!deleted)\\w+)");
            var regex2 = new Regex("pl_id=((?!deleted)\\w+)");
            var session = regex1.Match(headers).Groups[1].ToString();
            var plid = regex2.Match(headers).Groups[1].ToString();

            return new HwmCookies
            {
                SessionId = session,
                PlayerId = plid
            };
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

    }
}