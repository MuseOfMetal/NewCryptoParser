using CryptoParserSdk;
using CryptoParserSdk.Extensions;
using CryptoParserSdk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Parsers;

internal class CoinCatapult : CryptoParserAbstract
{
    public override string CryptocurrencyExchangeUrl => "https://coincatapult.com/";

    public override ParserConfig ParserConfig => new ParserConfig()
    {
        RequestRateType = ParserConfig.RequestRate.Minute,
        RequestsRate = 180,
        PrefixUrl = "https://coincatapult.com/coin/"
    };

    private HttpClient httpClient = new() { BaseAddress = new Uri("https://coincatapult.com/api/coins/") };

    public override CryptocurrencyInfo? GetCryptocurrencyInfo(string projectId, CryptocurrencyInfo info)
    {
        var coinInfo = JsonConvert.DeserializeObject<SocModel>(httpClient.GetStringAsync($"getObject?slug={projectId}").Result);

        info.Links.AddLink(coinInfo.response.social.discord, LinkType.Discord);
        info.Links.AddLink(coinInfo.response.social.website, LinkType.Website);
        info.Links.AddLink(coinInfo.response.social.telegram, LinkType.Telegram);
        info.Links.AddLink(coinInfo.response.social.twitter, LinkType.Twitter);

        info.Description = coinInfo.response.description;

        if (!string.IsNullOrEmpty(coinInfo.response.contract) || !string.IsNullOrEmpty(coinInfo.response.network))
            info.Platforms.Add(new Platform() { SmartContract = coinInfo.response.contract, Type = coinInfo.response.network });

        return info;
    }

    public override List<ParsingResult> GetCryptocurrencyList()
    {
        var coinList = JsonConvert.DeserializeObject<Model>(httpClient.GetStringAsync("getObjects").Result);

        return coinList.response.Select(x => new ParsingResult()
        {
            Name = x.name,
            Symbol = x.symbol,
            ProjectUrl = x.slug,
            ParamToSearchInfo = x.slug
        }).ToList();
    }
}

class SocModel
{
    public SocItem response { get; set; }
}
class SocItem
{
    public Social social { get; set; }
    public string contract { get; set; }
    public string description { get; set; }
    public string network { get; set; }
}
class Social
{
    public string telegram { get; set; }
    public string website { get; set; }
    public string twitter { get; set; }
    public string discord { get; set; }
}
class Model
{
    public List<Item> response { get; set; }
}
class Item
{
    public string name { get; set; }
    public string slug { get; set; }
    public string symbol { get; set; }
}