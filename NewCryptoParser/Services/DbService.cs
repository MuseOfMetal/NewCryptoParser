using Microsoft.EntityFrameworkCore;
using NewCryptoParser.Models;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace NewCryptoParser.Services
{
    public class DbService : DbContext
    {
        public DbService(DbContextOptions<DbService> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewCryptocurrencyProjectInfo>().Property(x => x.OtherInfo).HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(x) ?? new Dictionary<string, List<string>>()
                );
        }
        [NotNull]
        public DbSet<NewCryptocurrencyProject> NewCryptocurrencyProjects { get; set; }
    }
}
