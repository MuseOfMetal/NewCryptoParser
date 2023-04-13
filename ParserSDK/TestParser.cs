using CryptoParserSdk.Models;

namespace CryptoParserSdk
{
    public static class TestParser<T> where T : ICryptoParser, new()
    {
        public static void Test()
        {
            var parser = new T();
            Console.WriteLine($"Cryptocurrency Exchange Url: {parser.CryptocurrencyExchangeUrl}");
            Console.WriteLine($"Config:");
            Console.WriteLine($"\tRequest rate type: {parser.ParserConfig.RequestRateType}");
            Console.WriteLine($"\tRequest rate: {parser.ParserConfig.RequestsRate}");
            Console.WriteLine($"\tPrefix url: {parser.ParserConfig.PrefixUrl}");
            Console.WriteLine($"\tAdditional Parse Necessary: {parser.ParserConfig.AdditionalParseNecessary}");
        }


    }
}
