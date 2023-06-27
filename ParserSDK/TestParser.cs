using CryptoParserSdk.Models;

namespace CryptoParserSdk;

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
			if (projects.Count > 0)
			{
				IEnumerable<MultiQueryCryptocurrencyInfo[]> chunks;
				if (parser.ParserConfig.MultiQueryInfoLimit == 0)
					chunks = new List<MultiQueryCryptocurrencyInfo[]>()
					{
						projects
						.Select(x => new MultiQueryCryptocurrencyInfo() { ParamToSearchInfo = x.ParamToSearchInfo, CryptocurrencyInfo = x.CryptocurrencyInfo ?? new CryptocurrencyInfo() })
						.ToArray()
					};
				else
					chunks = projects
						.Select(x => new MultiQueryCryptocurrencyInfo() { ParamToSearchInfo = x.ParamToSearchInfo, CryptocurrencyInfo = x.CryptocurrencyInfo ?? new CryptocurrencyInfo() })
						.Chunk(parser.ParserConfig.MultiQueryInfoLimit);

				foreach (var chunk in chunks)
				{
					var infos = parser.GetCryptocurrenciesInfo(chunk.ToList());

					foreach (var item in infos)
					{
						var project = projects.First(x => x.ParamToSearchInfo == item.ParamToSearchInfo);
						Console.WriteLine($"Project name: {project.Name}");
						Console.WriteLine($"Project symbol: {project.Symbol}");
						Console.WriteLine($"Project url: {project.ProjectUrl}");
						Console.WriteLine($"Project param: {project.ParamToSearchInfo}");
						Console.WriteLine("Info:");
						var info = item.CryptocurrencyInfo;
						await Console.Out.WriteLineAsync($"Description: {project.CryptocurrencyInfo.Description}");
						await Console.Out.WriteLineAsync("---------LINKS---------");
						foreach (var link in project.CryptocurrencyInfo.Links)
						{
							await Console.Out.WriteLineAsync($"{(link.LinkType == 0 ? link.OtherLinkType : link.LinkType)}");
							foreach (var url in link.Urls)
								await Console.Out.WriteLineAsync(url);
							await Console.Out.WriteLineAsync("================================");
						}
						await Console.Out.WriteLineAsync("-----------------------");
						await Console.Out.WriteLineAsync("---------PLATFORMS---------");
						foreach (var platform in project.CryptocurrencyInfo.Platforms)
						{
							await Console.Out.WriteLineAsync($"Name: {platform.Name}");
							await Console.Out.WriteLineAsync($"Type: {platform.Type}");
							await Console.Out.WriteLineAsync($"Type: {platform.SmartContract}");
							await Console.Out.WriteLineAsync("==============================");
						}
						await Console.Out.WriteLineAsync("---------------------------");
					}
					await timer.WaitForNextTickAsync();
				}
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
				project.CryptocurrencyInfo = parser.GetCryptocurrencyInfo(project.ParamToSearchInfo, project.CryptocurrencyInfo ?? new CryptocurrencyInfo());
				await timer.WaitForNextTickAsync();
				await Console.Out.WriteLineAsync($"Description: {project.CryptocurrencyInfo.Description}");
				await Console.Out.WriteLineAsync("---------LINKS---------");
				foreach (var link in project.CryptocurrencyInfo.Links)
				{
					await Console.Out.WriteLineAsync($"{(link.LinkType == 0 ? link.OtherLinkType : link.LinkType)}");
					foreach (var url in link.Urls)
						await Console.Out.WriteLineAsync(url);
					await Console.Out.WriteLineAsync("================================");
				}
				await Console.Out.WriteLineAsync("-----------------------");
				await Console.Out.WriteLineAsync("---------PLATFORMS---------");
				foreach (var platform in project.CryptocurrencyInfo.Platforms)
				{
					await Console.Out.WriteLineAsync($"Name: {platform.Name}");
					await Console.Out.WriteLineAsync($"Type: {platform.Type}");
					await Console.Out.WriteLineAsync($"Contract: {platform.SmartContract}");
					await Console.Out.WriteLineAsync("==============================");
				}
				await Console.Out.WriteLineAsync("---------------------------");
			}
		}
	}
}