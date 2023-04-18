using NewCryptoParser.Abstract;
using NewCryptoParser.Services;

public static class ServiceProviderExtensions
{
    public static void AddParserManager(this IServiceCollection services)
    {
        services.AddSingleton<IParserManager, ParserManagerService>();
    }

    public static void AddNewCryptocurrencyProjectManager(this IServiceCollection services)
    {
        services.AddSingleton<INewCryptocurrencyProjectManager, NewCryptocurrencyProjectManagerService>();
    }
}