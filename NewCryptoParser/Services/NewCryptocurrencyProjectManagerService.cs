using CryptoParserSdk.Models;
using Microsoft.EntityFrameworkCore;
using NewCryptoParser.Abstract;
using NewCryptoParser.Models;
using System.Collections.Concurrent;

namespace NewCryptoParser.Services
{
    public class NewCryptocurrencyProjectManagerService : INewCryptocurrencyProjectManager
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<NewCryptocurrencyProjectManagerService> _logger;

        private ConcurrentQueue<(string parserName, ParsingResult project)> _buffer;


        public void AddNewProjects(string parserName, List<ParsingResult> projects)
        {
            foreach (var project in projects)
            {
                _buffer.Enqueue((parserName, project));
            }
        }

        public NewCryptocurrencyProject? GetLatestProject()
        {
            using var _context = getDbService();
            return _context.NewCryptocurrencyProjects.Last();
        }

        public NewCryptocurrencyProject? GetProjectById(int id)
        {
            using var _context = getDbService();
            return _context.NewCryptocurrencyProjects.Find(id);
        }

        public List<NewCryptocurrencyProject>? GetProjectsByIdRange(int startId, int endId)
        {
            throw new NotImplementedException();
        }

        public NewCryptocurrencyProjectManagerService(ILogger<NewCryptocurrencyProjectManagerService> logger, IServiceProvider provider)
        {
            _provider = provider.CreateScope().ServiceProvider;
            _logger = logger;
            _buffer = new ConcurrentQueue<(string parserName, ParsingResult project)>();
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        while (!_buffer.IsEmpty)
                        {
                            var _context = getDbService();
                            _logger.LogDebug("Creating db context");
                            _buffer.TryDequeue(out var data);
                            dataProccessor(data.parserName, data.project, _context);
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

        private void dataProccessor(string exchangeUrl, ParsingResult project, DbService _context)
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
                    Info = info
                });
            }
            else
            {
                if (finded.OtherInfos.FirstOrDefault(x => x.ExchangeUrl == exchangeUrl) == null)
                    finded.OtherInfos.Add(info);
            }
            _context.SaveChanges();
        }

        private DbService getDbService()
        {
            return _provider.CreateScope().ServiceProvider.GetService<DbService>();
        }
    }
}
