using CryptoParserSdk.Models;

namespace CryptoParserSdk;

public abstract class CryptoParserAbstract
{
    public abstract string CryptocurrencyExchangeUrl { get; }
    public abstract ParserConfig ParserConfig { get; }
    public abstract List<ParsingResult> GetCryptocurrencyList();
    public virtual CryptocurrencyInfo? GetCryptocurrencyInfo(string projectId, CryptocurrencyInfo info) { return default; }
    public virtual List<MultiQueryCryptocurrencyInfo>? GetCryptocurrenciesInfo(List<MultiQueryCryptocurrencyInfo> multiInfos) { return default; }
}