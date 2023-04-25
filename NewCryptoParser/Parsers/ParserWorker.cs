using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net;
using CryptoParserSdk;
using CryptoParserSdk.Models;
using System.Collections.Generic;

namespace ParserPlugin;
class ParserWorker : ICryptoParser
{

    public string Name => "Coindar";

    public string CryptocurrencyExchangeUrl => "https://coindar.org/";

    public ParserConfig ParserConfig => new()
    {
        RequestRateType = ParserConfig.RequestRate.Minute,
        RequestsRate = 5000
    };

    class Item
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symb { get; set; }
        public string ins { get; set; }
    }
    public List<ParsingResult> GetCryptocurrencyList()
    {

        return new List<ParsingResult>() 
        {
            new ParsingResult()
            {
                Name = "Name1",
                Symbol = "Symbol1",
                ProjectUrl = "Proj0ectUrl1",
                ParamToSearchInfo = "ParamToSearchInfo1",
            },
                        new ParsingResult()
            {
                Name = "Name2",
                Symbol = "Symbol2",
                ProjectUrl = "Proj0ectUrl2",
                ParamToSearchInfo = "ParamToSearchInfo2",
            },
                                    new ParsingResult()
            {
                Name = "Name3",
                Symbol = "Symbol3",
                ProjectUrl = "Proj0ectUrl3",
                ParamToSearchInfo = "ParamToSearchInfo3",
            }
        };
    }
     
    public CryptocurrencyInfo GetCryptocurrencyInfo(string projectId)
    {
        WebClient client = new WebClient();
        CryptocurrencyInfo socialLinks = new CryptocurrencyInfo();
        client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");
        var page = client.DownloadString("https://coindar.org/eu/coin/" + projectId);
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(page);
        var element = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div[2]/div/div[2]");
        string urls = null;

        foreach (var div in element.Elements("div"))
        {
            foreach (var a in div.Element("div").Elements("a"))
            {
                if (a.Attributes["href"].Value.Contains("https://t.me/"))
                {
                    socialLinks.TelegramUrls.Add(a.Attributes["href"].Value);
                }
            }
        }
        return socialLinks;
    }

    public ParserWorker()
    {
        
    }
}
