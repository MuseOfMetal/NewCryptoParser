using NewCryptoParser.Abstract;
using NewCryptoParser.Models;
using System.Collections.Concurrent;

namespace NewCryptoParser.Services;

public class NewCryptocurrencyProjectManagerService : INewCryptocurrencyProjectManager
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<NewCryptocurrencyProjectManagerService> _logger;
    private ConcurrentQueue<(string parserName, CryptoParserSdk.Models.ParsingResult project)> _buffer;

    public void AddNewProjects(string parserName, List<CryptoParserSdk.Models.ParsingResult> projects)
    {
        foreach (var project in projects)
        {
            _buffer.Enqueue((parserName, project));
        }
    }

    public IEnumerable<NewCryptocurrencyProject> GetProjects()
    {
        var _context = getDbService();
        return _context.NewCryptocurrencyProjects;
    }

    public NewCryptocurrencyProjectManagerService(ILogger<NewCryptocurrencyProjectManagerService> logger, IServiceProvider provider)
    {
        _provider = provider.CreateScope().ServiceProvider;
        _logger = logger;
        _buffer = new ConcurrentQueue<(string parserName, CryptoParserSdk.Models.ParsingResult project)>();
        new Thread(() =>
        {
            while (true)
            {
                try
                {
                    if (!_buffer.IsEmpty)
                    {
                        _logger.LogDebug("Creating db context");
                        var _context = getDbService();
                        while (!_buffer.IsEmpty)
                        {
                            _buffer.TryDequeue(out var data);
                            dataProccessor(data.parserName, data.project, _context);
                        }
                        _context.SaveChanges();
                        _context.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
                Thread.Sleep(100);
            }
        }).Start();
    }

    private void dataProccessor(string exchangeUrl, CryptoParserSdk.Models.ParsingResult project, NewCryptocurrencyDbContext _context)
    {
        var _projs = _context?.NewCryptocurrencyProjects;
        if (_projs == null)
        {
            _logger.LogError("List of new projects is null");
            return;
        }
        var finded = _projs.FirstOrDefault(p =>
        p.ProjectName.ToLower() == project.Name.ToLower() &&
        p.ProjectSymbol.ToLower() == project.Symbol.ToLower());

        //NewCryptocurrencyProjectInfo info = project.CryptocurrencyInfo as NewCryptocurrencyProjectInfo ?? new NewCryptocurrencyProjectInfo();
        NewCryptocurrencyProjectInfo info = new NewCryptocurrencyProjectInfo() 
        { 
            Links = project.CryptocurrencyInfo.Links,
            Platforms = project.CryptocurrencyInfo.Platforms,
            Start = project.CryptocurrencyInfo.Start,
            Description = project.CryptocurrencyInfo.Description,
            ExchangeUrl = exchangeUrl,
            ProjectUrl = project.ProjectUrl
        };
        if (finded == null)
        {
            _context.NewCryptocurrencyProjects.Add(new NewCryptocurrencyProject()
            {
                ExchangeUrl = exchangeUrl,
                ProjectName = project.Name,
                ProjectSymbol = project.Symbol,
                Info = info,
                ParseTime = DateTime.Now
            });
        }            
    }

    private NewCryptocurrencyDbContext getDbService()
    {
        return _provider.CreateScope().ServiceProvider.GetService<NewCryptocurrencyDbContext>()!;
    }
}