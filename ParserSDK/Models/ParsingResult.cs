namespace CryptoParserSdk.Models;
public class ParsingResult
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string ProjectUrl { get; set; } = string.Empty;
    public string ParamToSearchInfo { get; set; } = string.Empty;
    public CryptocurrencyInfo? CryptocurrencyInfo { get; set; }
}