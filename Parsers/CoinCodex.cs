using CryptoParserSdk;
using CryptoParserSdk.Extensions;
using CryptoParserSdk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ParserPlugin;

public class CoinCodex : CryptoParserAbstract
{
    public override string CryptocurrencyExchangeUrl => "https://coincodex.com/";

    public override ParserConfig ParserConfig => new ParserConfig()
    {
        RequestRateType = ParserConfig.RequestRate.Minute,
        RequestsRate = 180,
        PrefixUrl = "https://coincodex.com/crypto/"
    };

    private HttpClient httpClient = new HttpClient() { BaseAddress = new("https://coincodex.com/") };

    public override List<ParsingResult> GetCryptocurrencyList()
    {
        var coins = JsonConvert.DeserializeObject<List<CoinListModel.Coin>>(httpClient.GetStringAsync("apps/coincodex/cache/all_coins.json").Result) ?? new();

        return coins.Select(x => new ParsingResult()
        {
            Name = x.name,
            Symbol = x.display_symbol,
            ParamToSearchInfo = x.symbol,
            ProjectUrl = x.symbol
        }).ToList();
    }

    public override CryptocurrencyInfo? GetCryptocurrencyInfo(string projectId, CryptocurrencyInfo info)
    {
        var coin = JsonConvert.DeserializeObject<CoinInfoModel.Root>(httpClient.GetStringAsync($"api/coincodex/get_coin/{projectId}").Result) ?? new();

        info.Links.AddLink(coin.whitepaper, LinkType.TecnicalDoc);
        info.Links.AddLink(coin.website, LinkType.Website);
        info.Platforms.Add(new Platform() { Type = coin.platform });
        info.Description = coin.description;
        if (DateTime.TryParse(coin.start, out var data))
            info.Start = data;
        info.Links.SortLinks(coin.socials.Select(x => x.value).ToArray());

        return info;
    }

    class CoinListModel
    {
        public class Coin
        {
            public string symbol { get; set; } = string.Empty;
            public string display_symbol { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
        }
    }

    class CoinInfoModel
    {
        public class Root
        {
            public string description { get; set; }
            public string website { get; set; }
            public string whitepaper { get; set; }
            public string platform { get; set; }
            public string start { get; set; }
            public List<Social> socials { get; set; }
        }


        public class Social
        {
            public string value { get; set; }
        }
    }
}