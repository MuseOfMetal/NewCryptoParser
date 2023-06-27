using CryptoParserSdk;
using CryptoParserSdk.Extensions;
using CryptoParserSdk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ParserPlugin;

internal class CoinGecko : CryptoParserAbstract
{
    private HttpClient httpClient = new() { BaseAddress = new Uri("https://api.coingecko.com/api/v3/") };

    public override string CryptocurrencyExchangeUrl => "https://www.coingecko.com/";

    public override ParserConfig ParserConfig => new ParserConfig()
    {
        PrefixUrl = "https://www.coingecko.com/en/coins/",
        RequestRateType = ParserConfig.RequestRate.Minute,
        RequestsRate = 6
    };

    internal class Coin
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
    }

    internal class CoinInfoModel
    {
        public class Links
        {
            public List<string> homepage { get; set; } = new();
            public List<string> blockchain_site { get; set; } = new();
            public List<string> official_forum_url { get; set; } = new();
            public List<string> chat_url { get; set; } = new();
            public List<string> announcement_url { get; set; } = new();
            public string twitter_screen_name { get; set; }
            public string facebook_username { get; set; }
            public string telegram_channel_identifier { get; set; }
            public string subreddit_url { get; set; }
            public ReposUrl repos_url { get; set; } = new();
        }

        class DetailPlatforms
        {
            public Dictionary<string, List<Platform>> detail_platforms { get; set; } = new();
        }
        public class Platform
        {
            public string contract_address { get; set; }
        }
        public class ReposUrl
        {
            public List<string> github { get; set; } = new();
            public List<string> bitbucket { get; set; } = new();
        }

        public class Root
        {
            public Links links { get; set; } = new();
            public Dictionary<string, string> platforms { get; set; } = new();
            public object detail_platforms { get; set; } = new();
            public Dictionary<string, string> description { get; set; } = new();
            public string genesis_date { get; set; }
        }
    }

    public override List<ParsingResult> GetCryptocurrencyList()
    {
        var coins = JsonConvert.DeserializeObject<List<Coin>>(httpClient.GetStringAsync("coins/list").Result);
        List<ParsingResult> results = new List<ParsingResult>();

        foreach (var coin in coins)
        {
            results.Add(new ParsingResult()
            {
                Name = coin.name,
                Symbol = coin.symbol,
                ProjectUrl = coin.id,
                ParamToSearchInfo = coin.id
            });
        }

        return results;
    }

    public override CryptocurrencyInfo? GetCryptocurrencyInfo(string projectId, CryptocurrencyInfo info)
    {
        var coinInfo = JsonConvert.DeserializeObject<CoinInfoModel.Root>(httpClient.GetStringAsync($"coins/{projectId}").Result);

        info.Platforms.AddRange(coinInfo.platforms.Select(x => new Platform() { Name = x.Key, SmartContract = x.Value }).ToList());

        info.Links.AddLinks(LinkType.Website, coinInfo.links.homepage.ToArray());

        if (!string.IsNullOrEmpty(coinInfo.links.facebook_username))
            info.Links.AddLink("https://www.facebook.com/" + coinInfo.links.facebook_username, LinkType.Facebook);

        info.Links.AddLinks(LinkType.Website, coinInfo.links.repos_url.github.ToArray());
        info.Links.AddLinks(LinkType.Website, coinInfo.links.repos_url.bitbucket.ToArray());

        if (!string.IsNullOrEmpty(coinInfo.links.twitter_screen_name))
            info.Links.AddLink("https://twitter.com/" + coinInfo.links.twitter_screen_name, LinkType.Twitter);

        if (!string.IsNullOrEmpty(coinInfo.links.telegram_channel_identifier))
            info.Links.AddLink("https://t.me/" + coinInfo.links.telegram_channel_identifier, LinkType.Telegram);

        if (!string.IsNullOrEmpty(coinInfo.links.subreddit_url))
            info.Links.AddLink(coinInfo.links.subreddit_url, LinkType.Reddit);

        info.Links.AddLinks(LinkType.Explorer, coinInfo.links.blockchain_site.ToArray());

        List<string> unknownLinks = new List<string>();
        unknownLinks.AddRange(coinInfo.links.official_forum_url.Where(x => !string.IsNullOrEmpty(x)));
        unknownLinks.AddRange(coinInfo.links.chat_url.Where(x => !string.IsNullOrEmpty(x)));
        unknownLinks.AddRange(coinInfo.links.announcement_url.Where(x => !string.IsNullOrEmpty(x)));
        if (coinInfo.description.TryGetValue("en", out var data))
            info.Description = data;
        info.Links.SortLinks(unknownLinks.ToArray());
        if (coinInfo.genesis_date != null)
            if (DateTime.TryParse(coinInfo.genesis_date, out var dt))
                info.Start = dt;
        return info;
    }
}