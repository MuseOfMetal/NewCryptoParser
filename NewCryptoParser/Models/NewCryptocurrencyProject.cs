using CryptoParserSdk.Models;

namespace NewCryptoParser.Models
{
    public class NewCryptocurrencyProject
    {
        public int Id { get; set; }
        public string ParserName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectSymbol { get; set; } = string.Empty;
        public List<NewCryptocurrencyProjectInfo> Infos { get; set; } = new();

    }
}
