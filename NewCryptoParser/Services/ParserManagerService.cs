using CryptoParserSdk;
using NewCryptoParser.Abstract;
using System.Collections.Concurrent;
using NewCryptoParser.Models;
using CryptoParserSdk.Models;
using NewCryptoParser.Exceptions;

namespace NewCryptoParser.Services
{
    public class ParserManagerService : IParserManager
    {
        private static ConcurrentDictionary<string, CryptoParserScheduledTask> parsers;
        private readonly ILogger<ParserManagerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public void AddParser(string code, string name)
        {
            if (parsers.ContainsKey(name))
                throw new ParserAlreadyExist();
            var successAdd = parsers.TryAdd(name, createCryptoParserScheduledTask(code, name));
            if (!successAdd)
                throw new ParserAddException("ConcurrentDictionary.TryAdd() return false");
        }

        public CryptoParserScheduledTask? GetParser(string name)
        {
            var successGet = parsers.TryGetValue(name, out var parser);
            if (!successGet)
                throw new ParserGetException("ConcurrentDictionary.TryGet() return false");
            return parser;
        }

        public void RemoveParser(string name)
        {
            var successRemove = parsers.TryRemove(name, out var parser);
            if (!successRemove)
                throw new ParserRemovalException("ConcurrentDictionary.TryRemove() return false");
            parser.Dispose();
        }

        public void UpdateParser(string code, string name)
        {
            var getSuccess = parsers.TryGetValue(name, out var parser);
            if (getSuccess)
            {
                parser.Dispose();
                var successUpdate = parsers.TryUpdate(name, createCryptoParserScheduledTask(code, name), parser);
                if (!successUpdate)
                    throw new ParserRemovalException("ConcurrentDictionary.TryUpdate() return false");
            }
            else
                AddParser(code, name);
        }

        private CryptoParserScheduledTask createCryptoParserScheduledTask(string code, string name)
        {
            var cryptoTask = new CryptoParserScheduledTask();
            cryptoTask.CancellationTokenSource = new CancellationTokenSource();
            cryptoTask.CryptoParser = CodeCompiler.CompileCodeAndGetObject<ICryptoParser>(code); ;
            cryptoTask.PeriodicTask = new Task(async _ =>
            {
                var _parser = cryptoTask.CryptoParser;
                var _name = name;
                var _localProjectsRepository = _parser.GetCryptocurrencyList();

                TimeSpan interval =
                TimeSpan.FromMilliseconds((int)_parser.ParserConfig.RequestRateType /
                _parser.ParserConfig.RequestsRate);

                PeriodicTimer timer = new PeriodicTimer(interval);

                while (
                !cryptoTask.CancellationTokenSource.IsCancellationRequested &&
                await timer.WaitForNextTickAsync()
                )
                {
                    var projects = _parser.GetCryptocurrencyList();
                    var newProjects = CheckProjects(projects, _localProjectsRepository);

                    foreach (var newProject in newProjects)
                        if (newProject.CryptocurrencyInfo == null)
                            newProject.CryptocurrencyInfo =
                            _parser.GetCryptocurrencyInfo(newProject.ParamToSearchInfo);

                    _localProjectsRepository = newProjects;
                    using var scope = _serviceProvider.CreateScope();
                    var _newCryptocurrencyProjectManager =
                    scope.ServiceProvider.GetService<INewCryptocurrencyProjectManager>();
                    _newCryptocurrencyProjectManager.AddNewProjects(_name, _localProjectsRepository);
                }
            }, cryptoTask.CancellationTokenSource);

            return cryptoTask;

            static List<ParsingResult> CheckProjects(
                List<ParsingResult> parsingResults,
                List<ParsingResult> repository
                )
            {
                var newProjects = new List<ParsingResult>();
                foreach (var project in parsingResults)
                    if (repository.FirstOrDefault(p => p.ProjectUrl == project.ProjectUrl) == null)
                        newProjects.Add(project);
                return newProjects;
            }
        }

        public ParserManagerService(
            ILogger<ParserManagerService> logger,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        static ParserManagerService()
        {
            parsers = new ConcurrentDictionary<string, CryptoParserScheduledTask>();
        }
    }
}