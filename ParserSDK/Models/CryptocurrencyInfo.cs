namespace CryptoParserSdk.Models;

public class CryptocurrencyInfo
{
    public DateTime Start { get; set; }
    public List<Platform> Platforms { get; set; } = new();
    public string? Description { get; set; }
    public List<Link> Links { get; set; } = new();

    public void AddLink(string url, LinkType type = 0, string otherType = "Unknown")
    {
        var link = Links.FirstOrDefault(x => x.LinkType == type);
        if (link == null)
        {
            var newLink = new Link();
            newLink.Urls.Add(url);
            newLink.LinkType = type;
            if (type == 0)
                otherType = "Unknown";
            Links.Add(newLink);
        }
        else
            link.Urls.Add(url);
    }

    //    public void AddPlatform(string? name = null, string? type = null, string? smartContract = null)
    //    {


    //    }
    //
}