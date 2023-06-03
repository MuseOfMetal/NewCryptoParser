using CryptoParserSdk;
using CryptoParserSdk.Models;
using ParserPlugin;
//using ParserPlugin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parsers;

internal class Program
{
    static async Task Main(string[] args)
    {
        await TestParser<CoinCatapult>.Test();
    }
}
