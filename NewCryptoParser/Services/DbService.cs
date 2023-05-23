using Microsoft.EntityFrameworkCore;
using NewCryptoParser.Models;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace NewCryptoParser.Services;

public class DbService : DbContext
{
    public DbService(DbContextOptions<DbService> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewCryptocurrencyProject>().Property(x => x.Info).HasConversion
            (
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<NewCryptocurrencyProjectInfo>(x) ?? new()
            );
        modelBuilder.Entity<NewCryptocurrencyProject>().Property(x => x.OtherInfos).HasConversion
            (
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<List<NewCryptocurrencyProjectInfo>>(x) ?? new()
            );
    }
    [NotNull]
    public DbSet<NewCryptocurrencyProject> NewCryptocurrencyProjects { get; set; }
}
