namespace DataImportService.Parsers
{
    using System.Net.Http;
    using System.Text;
    using HtmlAgilityPack;

    public class ParseTransferProtocol : IParseTransferProtocol
    {
        public void Execute(HttpClient client)
        {
            var res = client.GetAsync("https://www.heroeswm.ru/pl_transfers.php?id=2002274").Result.Content.ReadAsByteArrayAsync().Result;
            var encoding = Encoding.GetEncoding(1251);
            var htmlString = encoding.GetString(res);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            Parse(htmlDocument);
        }

        private void Parse(HtmlDocument html)
        {
            var htmlNodeCollection = html.DocumentNode.SelectNodes("//table//tr/td/br/center");
        }
    }

    public interface IParseTransferProtocol
    {
    }
}