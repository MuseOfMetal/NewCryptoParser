namespace CryptoParserSdk.Models;

public class ParserConfig
{
    public enum RequestRate
    {
        Minute = 60_000,
        Hour = 3600_000,
        Day = 86400_000,
        Week = 604800_000,
    }
    public string? PrefixUrl { get; set; }
    public RequestRate RequestRateType { get; set; }
    public int RequestsRate { get; set; }
    public bool AdditionalParseNecessary { get; set; }
    public object? Custom { get; set; }
}
