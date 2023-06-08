using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class NewCryptocurrencyProject
{
    [Key]
    public int Id { get; set; }
    public string ExchangeUrl { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectSymbol { get; set; } = string.Empty;
    public DateTime ParseTime { get; set; }
    public NewCryptocurrencyProjectInfo Info { get; set; } = new();
}