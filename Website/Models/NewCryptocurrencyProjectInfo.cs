using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class NewCryptocurrencyProjectInfo : CryptoParserSdk.Models.CryptocurrencyInfo
{
    [Key]
    public int Id { get; set; }
    public string ExchangeUrl { get; set; } = string.Empty;
    public string ProjectUrl { get; set; } = string.Empty;
}