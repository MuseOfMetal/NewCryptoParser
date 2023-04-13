namespace CryptoParserSdk.Models;

public class ParserConfig
{
    public enum RequestRate
    {
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year,
    }
    public string? PrefixUrl { get; set; }
    public RequestRate RequestRateType { get; set; }
    public int RequestsRate { get; set; }
    public bool AdditionalParseNecessary { get; set; }
    public object? Custom { get; set; }
}
