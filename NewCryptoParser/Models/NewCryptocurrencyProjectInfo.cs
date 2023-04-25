using CryptoParserSdk.Models;
using System.ComponentModel.DataAnnotations;

namespace NewCryptoParser.Models
{
    public class NewCryptocurrencyProjectInfo : CryptocurrencyInfo
    {
        [Key]
        public int Id { get; set; }
        public string ParserName { get; set; } = string.Empty;
        public string ProjectUrl { get; set; } = string.Empty;
    }
}
