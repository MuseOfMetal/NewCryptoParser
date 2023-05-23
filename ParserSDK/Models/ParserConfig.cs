namespace CryptoParserSdk.Models;

public class ParserConfig
{
    public enum RequestRate
    {
        Minute = 60,
        Hour = 3600,
        Day = 86400,
        Week = 604800,
        Month = 2592000
    }
    public string? PrefixUrl { get; set; }
    public RequestRate RequestRateType { get; set; }
    public int RequestsRate { get; set; }
    public object? Custom { get; set; }
    public bool MultiQueryInfoSupport { get; set; } = false;
}
