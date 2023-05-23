using CryptoParserSdk;
using CryptoParserSdk.Models;
//using ParserPlugin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parsers;

internal class Program
{
    static async Task Main(string[] args)
    {
        //await TestParser<CoinMarketCap>.Test();
    }
}

class Parser : CryptoParserAbstract
{
    public override string CryptocurrencyExchangeUrl => throw new NotImplementedException();

    public override ParserConfig ParserConfig => throw new NotImplementedException();

    public override List<ParsingResult> GetCryptocurrencyList()
    {
        throw new NotImplementedException();
    }
}