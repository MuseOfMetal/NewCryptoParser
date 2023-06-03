using CryptoParserSdk;
using CryptoParserSdk.Extensions;
using CryptoParserSdk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ParserPlugin
{
    internal class CoinMarketCap : CryptoParserAbstract
    {
        public override string CryptocurrencyExchangeUrl => "https://coinmarketcap.com/";

        private HttpClient httpClient { get; set; }

        public override ParserConfig ParserConfig => new ParserConfig()
        {
            PrefixUrl = "https://coinmarketcap.com/currencies/",
            RequestRateType = ParserConfig.RequestRate.Month,
            RequestsRate = 10000,
            MultiQueryInfoSupport = true,
            MultiQueryInfoLimit = 100
        };

        public CoinMarketCap()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://pro-api.coinmarketcap.com/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", "da168ac5-ba03-4eed-856f-74302427de53");
        }
        public CryptocurrencyInfo GetCryptocurrencyInfo(string projectId)
        {
            throw new NotImplementedException();
        }


        public override List<ParsingResult> GetCryptocurrencyList()
        {
            var json = httpClient.GetStringAsync("/v1/cryptocurrency/map").Result;
            var coinList = JsonConvert.DeserializeObject<CoinListModel.Root>(json);

            return coinList.data.Select(x => new ParsingResult()
            {
                Name = x.name,
                Symbol = x.symbol,
                ProjectUrl = x.symbol,
                ParamToSearchInfo = $"{x.id}",
                CryptocurrencyInfo = new CryptocurrencyInfo()
                {
                    Platforms = new List<Platform>() 
                    { new Platform()
                        {
                            Name = x.platform?.name,
                            SmartContract = x.platform?.token_address
                        }
                    }
                }

            }).ToList();
        }

        public override List<MultiQueryCryptocurrencyInfo> GetCryptocurrenciesInfo(List<MultiQueryCryptocurrencyInfo> multiInfos)
        {
            string query = string.Join(',', multiInfos.Select(x => x.ParamToSearchInfo));
            var coinInfos = JsonConvert.DeserializeObject<CoinInfoModel.Root>(httpClient.GetStringAsync($"/v2/cryptocurrency/info?id={query}").Result);

            foreach (var key in coinInfos.data.Keys)
            {
                var multiInfo = multiInfos.First(x => x.ParamToSearchInfo == key);
                var info = multiInfo.CryptocurrencyInfo;
                var coinInfo = coinInfos.data[key];
                info.Links.AddLinks(LinkType.Website, coinInfo.urls.website);
                info.Links.AddLinks(LinkType.Explorer, coinInfo.urls.explorer);
                info.Links.AddLinks(LinkType.SourceCode, coinInfo.urls.source_code);
                info.Links.AddLinks(LinkType.Reddit, coinInfo.urls.reddit);
                info.Links.AddLinks(LinkType.Twitter, coinInfo.urls.twitter);
                List<string> unknownLinks = new List<string>();
                unknownLinks.AddRange(coinInfo.urls.announcement);
                unknownLinks.AddRange(coinInfo.urls.technical_doc);
                unknownLinks.AddRange(coinInfo.urls.message_board);
                unknownLinks.AddRange(coinInfo.urls.chat);
                info.Links.SortLinks(unknownLinks.ToArray());
                if (!string.IsNullOrEmpty(coinInfo.description))
                    info.Description = coinInfo.description;
            }
            return multiInfos;
        }
    }

    public class CoinListModel
    {
        public class Datum
        {
            public int id { get; set; }
            public int rank { get; set; }
            public string name { get; set; }
            public string symbol { get; set; }
            public string slug { get; set; }
            public int is_active { get; set; }
            public DateTime first_historical_data { get; set; }
            public DateTime last_historical_data { get; set; }
            public Platform platform { get; set; }
        }

        public class Platform
        {
            public int id { get; set; }
            public string name { get; set; }
            public string symbol { get; set; }
            public string slug { get; set; }
            public string token_address { get; set; }
        }

        public class Root
        {
            public List<Datum> data { get; set; }
            public Status status { get; set; }
            public Platform platform { get; set; }
        }

        public class Status
        {
            public DateTime timestamp { get; set; }
            public int error_code { get; set; }
            public string error_message { get; set; }
            public int elapsed { get; set; }
            public int credit_count { get; set; }
        }
    }

    public class CoinInfoModel
    {
        public class Root
        {
            public Dictionary<string, Coin> data { get; set; }
        }

        public class Coin
        {
            public Url urls { get; set; }
            public string description { get; set; }
        }
        public class Url
        {
            public string[] website { get; set; }
            public string[] technical_doc { get; set; }
            public string[] explorer { get; set; }
            public string[] source_code { get; set; }
            public string[] message_board { get; set; }
            public string[] chat { get; set; }
            public string[] announcement { get; set; }
            public string[] reddit { get; set; }
            public string[] twitter { get; set; }
        }
    }
}
