namespace CryptoParserSdk.Models;

public class CryptocurrencyInfo
{
    public DateTime Start { get; set; }
    public List<Platform> Platforms { get; set; } = new();
    public string? Description { get; set; }
    public List<Link> Links { get; set; } = new();


}