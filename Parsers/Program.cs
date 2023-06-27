using CryptoParserSdk;
using ParserPlugin;
using System.Threading.Tasks;

namespace Parsers;

internal class Program
{
    static async Task Main(string[] args)
    {
        await TestParser<CoinPaprika>.Test();
    }
}