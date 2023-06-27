namespace CryptoParserSdk.Models;

public class Link
{
    public LinkType LinkType { get; set; }
    public List<string> Urls { get; set; } = new();
    public string? OtherLinkType { set; get; }
}
public enum LinkType
{
    Other = 0,
    CoinGecko,
    CoinMarketCap,
    Discord,
    Email,
    Explorer,
    Facebook,
    Instagram,
    LinkedIn,
    Medium,
    Quora,
    Website,
    SourceCode,
    Reddit,
    Slack,
    TecnicalDoc,
    Telegram,
    TikTok,
    Twitch,
    Twitter,
    Youtube,
}