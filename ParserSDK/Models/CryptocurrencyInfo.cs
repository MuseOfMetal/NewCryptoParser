namespace CryptoParserSdk.Models;

public class CryptocurrencyInfo
{
    public List<string> CoinGeckoUrls { get; set; }                        = new();
    public List<string> CoinMarketCapUrls { get; set; }                    = new();
    public List<string> DiscordUrls { get; set; }                          = new();
    public List<string> Emails { get; set; }                                = new();
    public List<string> ExplorerUrls { get; set; }                         = new();
    public List<string> FacebookUrls { get; set; }                         = new();
    public List<string> MediumUrls { get; set; }                           = new();
    public List<string> WebsiteUrls { get; set; }                          = new();
    public List<string> SmartContract { get; set; }                        = new();
    public List<string> SourceCodeUrls { get; set; }                       = new();
    public List<string> RedditUrls { get; set; }                           = new();
    public List<string> TelegramUrls { get; set; }                         = new();
    public List<string> TwitterUrls { get; set; }                          = new();
    public List<string> YoutubeUrls { get; set; }                          = new();
    public Dictionary<string, List<string>> OtherInfo { get; set; }        = new();
}
