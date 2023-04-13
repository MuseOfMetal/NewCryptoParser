using CryptoParserSdk.Models;

namespace NewCryptoParser.Models
{
    public class NewCryptocurrencyProjectInfo : CryptocurrencyInfo
    {
        public string ParserName { get; set; } = string.Empty;
        public string ProjectUrl { get; set; } = string.Empty;
    }
}
