namespace Website;

public class ParserInfoService
{
    private readonly ILogger<ParserInfoService> _logger;

    private Dictionary<string, (string name, string imageUrl)> parserInfo = new Dictionary<string, (string, string)> 
    { 
        { "https://coinpaprika.com/", ("CoinPaprika", "./images/coinpaprika.png") },
        { "https://coinmarketcap.com/", ("CoinMarketCap", "./images/coinmarketcap.webp") },
        { "https://www.coingecko.com/", ("CoinGecko", "./images/coingecko.png") },
        { "https://coincatapult.com/", ("CoinCatapult", "./images/coincatapult.png") },
        { "https://coincodex.com/", ("CoinCodex", "./images/coincodex.webp") }
    };

    public string? GetName(string exchangeUrl)
    {
        if (parserInfo.TryGetValue(exchangeUrl, out var info))
            return info.name;
        return null;
    }

    public string? GetImageUrl(string exchangeUrl)
    {
        if (parserInfo.TryGetValue(exchangeUrl, out var info))
            return info.imageUrl;
        return null;
    }

    public ParserInfoService(ILogger<ParserInfoService> logger)
    {
        _logger = logger;
    }
}