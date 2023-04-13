using NewCryptoParser.Abstract;
using NewCryptoParser.Services;

public static class ServiceProviderExtensions
{
    public static void AddParserManager(this IServiceCollection services)
    {
        //services.AddSingleton<IParserManager, ParserManagerService>();
    }
}