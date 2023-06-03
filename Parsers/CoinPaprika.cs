using CryptoParserSdk;
using CryptoParserSdk.Extensions;
using CryptoParserSdk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ParserPlugin;

internal class CoinPaprika : CryptoParserAbstract
{
    public override string CryptocurrencyExchangeUrl => "https://coinpaprika.com/";
    
    public override ParserConfig ParserConfig => new ParserConfig()
    {
        RequestRateType = ParserConfig.RequestRate.Month,
        RequestsRate = 300000,
        PrefixUrl = "https://coinpaprika.com/coin/"
    };

    private HttpClient httpClient = new() { BaseAddress = new Uri("https://api.coinpaprika.com/v1/") };

    public override CryptocurrencyInfo? GetCryptocurrencyInfo(string projectId, CryptocurrencyInfo info)
    {
        var coin = JsonConvert
            .DeserializeObject<CoinInfo>(httpClient.GetAsync($"coins/{projectId}").Result.Content.ReadAsStringAsync().Result);

        if (coin == null)
            return new();

        info.Links.AddLinks(LinkType.Explorer, coin.links.explorer?.ToArray());
        info.Links.AddLinks(LinkType.Facebook, coin.links.facebook?.ToArray());
        info.Links.AddLinks(LinkType.Reddit, coin.links.reddit?.ToArray());
        info.Links.AddLinks(LinkType.SourceCode, coin.links.source_code?.ToArray());
        info.Links.AddLinks(LinkType.Website, coin.links.website?.ToArray());
        info.Links.AddLinks(LinkType.Youtube, coin.links.youtube?.ToArray());
        info.Links.AddLinks(LinkType.Twitter, coin.links_extended.Where(x => x.type == "twitter").Select(x => x.url)?.ToArray());
        info.Links.AddLinks(LinkType.Telegram, coin.links_extended.Where(x => x.type == "telegram").Select(x => x.url)?.ToArray());
        info.Links.AddLinks(LinkType.Quora, coin.links_extended.Where(x => x.type == "quora").Select(x => x.url)?.ToArray());
        info.Links.AddLinks(LinkType.TecnicalDoc, coin.whitepaper?.link);
        
        info.Description = coin.description;


        foreach (var contract in coin.contracts ?? new List<Contract>())
        {
            info.Platforms.Add(new Platform() { Name = contract.platform, SmartContract = contract.contract, Type = contract.type });
        }

        if (DateTime.TryParse(coin.started_at, out var time))
            info.Start = time;           

        return info;
    }

    public override List<ParsingResult> GetCryptocurrencyList()
    {
        var json = httpClient.GetAsync("coins").Result.Content.ReadAsStringAsync().Result;
        var coins = JsonConvert
            .DeserializeObject<List<CoinList>>(json)
            ?.Where(x=>x.IsNew);

        if (coins == null)
            return new();

        return coins.Select(coin => new ParsingResult
        {
            Name = coin.Name,
            Symbol = coin.Symbol,
            ProjectUrl = coin.Id,
            ParamToSearchInfo = coin.Id
        }).ToList();
    }
}

class CoinList
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;
    [JsonProperty("rank")]
    public string Rank { get; set; } = string.Empty;
    [JsonProperty("is_new")]
    public bool IsNew { get; set; }
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
}
public class Links
{
    public List<string> explorer { get; set; }
    public List<string> facebook { get; set; }
    public List<string> reddit { get; set; }
    public List<string> source_code { get; set; }
    public List<string> website { get; set; }
    public List<string> youtube { get; set; }
    public List<string> medium { get; set; }
}

public class LinksExtended
{
    public string url { get; set; }
    public string type { get; set; }
    public Stats stats { get; set; }
}

public class CoinInfo
{
    public string id { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public int rank { get; set; }
    public bool is_new { get; set; }
    public bool is_active { get; set; }
    public string type { get; set; }
    public string logo { get; set; }
    public List<Tag> tags { get; set; }
    public List<Contract> contracts { get; set; } = new();
    public List<Team> team { get; set; }
    public string description { get; set; }
    public string started_at { get; set; }
    public string message { get; set; }
    public bool open_source { get; set; }
    public string development_status { get; set; }
    public bool hardware_wallet { get; set; }
    public string proof_type { get; set; }
    public string org_structure { get; set; }
    public string hash_algorithm { get; set; }
    public Links links { get; set; } = new Links(); 
    public List<LinksExtended> links_extended { get; set; } = new ();
    public Whitepaper whitepaper { get; set; }
}

public class Contract
{
    public string contract { get; set; }
    public string platform { get; set; }
    public string type { get; set; }
}
public class Stats
{
    public int subscribers { get; set; }
    public int? contributors { get; set; }
    public int? stars { get; set; }
    public int? followers { get; set; }
}

public class Tag
{
    public string id { get; set; }
    public string name { get; set; }
    public int coin_counter { get; set; }
    public int ico_counter { get; set; }
}

public class Team
{
    public string id { get; set; }
    public string name { get; set; }
    public string position { get; set; }
}

public class Whitepaper
{
    public string link { get; set; }
    public string thumbnail { get; set; }
}