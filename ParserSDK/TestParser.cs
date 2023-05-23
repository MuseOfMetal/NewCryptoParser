using CryptoParserSdk.Models;

namespace CryptoParserSdk
{
    public static class TestParser<T> where T : CryptoParserAbstract, new()
    {
        public static async Task Test()
        {
            var parser = new T();
            Console.WriteLine($"Cryptocurrency Exchange Url: {parser.CryptocurrencyExchangeUrl}");
            Console.WriteLine($"Config:");
            Console.WriteLine($"\tRequest rate type: {parser.ParserConfig.RequestRateType}");
            Console.WriteLine($"\tRequest rate: {parser.ParserConfig.RequestsRate}");
            Console.WriteLine($"\tPrefix url: {parser.ParserConfig.PrefixUrl}");
            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds((double)parser.ParserConfig.RequestRateType / parser.ParserConfig.RequestsRate));
            var projects = parser.GetCryptocurrencyList();
            await timer.WaitForNextTickAsync();


            if (parser.ParserConfig.MultiQueryInfoSupport)
            {
                var infos = parser.GetCryptocurrenciesInfo(projects.Select(x => x.ParamToSearchInfo).ToArray());
                await timer.WaitForNextTickAsync();
                foreach (var item in infos)
                {
                    var project = projects.First(x => x.ParamToSearchInfo == item.ParamToSearchInfo);
                    Console.WriteLine($"Project name: {project.Name}");
                    Console.WriteLine($"Project symbol: {project.Symbol}");
                    Console.WriteLine($"Project url: {project.ProjectUrl}");
                    Console.WriteLine($"Project param: {project.ParamToSearchInfo}");
                    Console.WriteLine("Info:");
                    var info = item.CryptocurrencyInfo;

                }
            }
            else
            {
                foreach (var project in projects)
                {
                    Console.WriteLine($"Project name: {project.Name}");
                    Console.WriteLine($"Project symbol: {project.Symbol}");
                    Console.WriteLine($"Project url: {project.ProjectUrl}");
                    Console.WriteLine($"Project param: {project.ParamToSearchInfo}");
                    Console.WriteLine("Info:");
                    await Console.Out.WriteLineAsync($"Description: {project.CryptocurrencyInfo.Description}");
                    await Console.Out.WriteLineAsync("---------LINKS---------");
                    foreach (var link in project.CryptocurrencyInfo.Links)
                    {
                        await Console.Out.WriteLineAsync($"{(link.LinkType == 0 ? link.OtherLinkType : link.LinkType)}");
                        foreach (var url in link.Urls)
                        {
                            await Console.Out.WriteLineAsync(url);
                        }
                        await Console.Out.WriteLineAsync("================================");
                    }
                    await Console.Out.WriteLineAsync("-----------------------");
                    await Console.Out.WriteLineAsync("---------PLATFORMS---------");
                    foreach (var item in project.CryptocurrencyInfo.Platforms)
                    {
                        await Console.Out.WriteLineAsync($"Name: {item.Name}");
                        await Console.Out.WriteLineAsync($"Type: {item.Type}");
                        await Console.Out.WriteLineAsync($"Type: {item.SmartContract}");
                        await Console.Out.WriteLineAsync("==============================");
                    }
                    await Console.Out.WriteLineAsync("---------------------------");
                }
            }


            //        if (parser.ParserConfig.MultiQueryInfoSupport)
            //{
            //    var infos = parser.GetCryptocurrenciesInfo(projects.Select(x=>x.ParamToSearchInfo).ToArray());
            //    await timer.WaitForNextTickAsync();
            //    foreach (var item in infos)
            //    {
            //        var project = projects.First(x => x.ParamToSearchInfo == item.ParamToSearchInfo);
            //        Console.WriteLine($"Project name: {project.Name}");
            //        Console.WriteLine($"Project symbol: {project.Symbol}");
            //        Console.WriteLine($"Project url: {project.ProjectUrl}");
            //        Console.WriteLine($"Project param: {project.ParamToSearchInfo}");
            //        Console.WriteLine("Info:");
            //        var info = item.CryptocurrencyInfo;

            //        print("CoinGeckoUrls", info.CoinGeckoUrls);
            //        print("CoinMarketCapUrls", info.CoinMarketCapUrls);
            //        print("DiscordUrls", info.DiscordUrls);
            //        print("Emails", info.Emails);
            //        print("ExplorerUrls", info.ExplorerUrls);
            //        print("FacebookUrls", info.FacebookUrls);
            //        print("MediumUrls", info.MediumUrls);
            //        print("WebsiteUrls", info.WebsiteUrls);
            //        print("SmartContract", info.SmartContract);
            //        print("SourceCodeUrls", info.SourceCodeUrls);
            //        print("RedditUrls", info.RedditUrls);
            //        print("TelegramUrls", info.TelegramUrls);
            //        print("TwitterUrls", info.TwitterUrls);
            //        print("YoutubeUrls", info.YoutubeUrls);
            //        Console.WriteLine("Other info:");
            //        foreach (var key in info.OtherInfo.Keys)
            //        {
            //            Console.WriteLine(key);
            //            Console.WriteLine("* * *");
            //            foreach (var data in info.OtherInfo[key])
            //            {
            //                Console.WriteLine(data);
            //            }
            //            Console.WriteLine("* * *");
            //        }
            //        Console.WriteLine(new string('=', 40));
            //    }
            //}
            //else
            //{
            //    foreach (var project in projects)
            //    {
            //        Console.WriteLine($"Project name: {project.Name}");
            //        Console.WriteLine($"Project symbol: {project.Symbol}");
            //        Console.WriteLine($"Project url: {project.ProjectUrl}");
            //        Console.WriteLine($"Project param: {project.ParamToSearchInfo}");
            //        Console.WriteLine("Info:");

            //        var info = parser.GetCryptocurrencyInfo(project.ParamToSearchInfo);
            //        await timer.WaitForNextTickAsync();
            //        print("CoinGeckoUrls", info.CoinGeckoUrls);
            //        print("CoinMarketCapUrls", info.CoinMarketCapUrls);
            //        print("DiscordUrls", info.DiscordUrls);
            //        print("Emails", info.Emails);
            //        print("ExplorerUrls", info.ExplorerUrls);
            //        print("FacebookUrls", info.FacebookUrls);
            //        print("MediumUrls", info.MediumUrls);
            //        print("WebsiteUrls", info.WebsiteUrls);
            //        print("SmartContract", info.SmartContract);
            //        print("SourceCodeUrls", info.SourceCodeUrls);
            //        print("RedditUrls", info.RedditUrls);
            //        print("TelegramUrls", info.TelegramUrls);
            //        print("TwitterUrls", info.TwitterUrls);
            //        print("YoutubeUrls", info.YoutubeUrls);
            //        Console.WriteLine("Other info:");
            //        foreach (var key in info.OtherInfo.Keys)
            //        {
            //            Console.WriteLine(key);
            //            Console.WriteLine("* * *");
            //            foreach (var data in info.OtherInfo[key])
            //            {
            //                Console.WriteLine(data);
            //            }
            //            Console.WriteLine("* * *");
            //        }
            //        Console.WriteLine(new string('=', 40));
            //    }
            //}

        }

        private static void print(string name, List<string> texts)
        {
            Console.WriteLine(name);
            foreach (var item in texts)
            {
                Console.WriteLine(item);
            }
        }


    }
}
