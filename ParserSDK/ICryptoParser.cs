using CryptoParserSdk.Models;

namespace CryptoParserSdk;

public interface ICryptoParser
{
    string CryptocurrencyExchangeUrl { get; }
    ParserConfig ParserConfig { get; }
    List<ParsingResult> GetCryptocurrencyList();
    CryptocurrencyInfo GetCryptocurrencyInfo(string projectId);
}