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
        private ConcurrentDictionary<string, CryptoParserScheduledTask> parsers;
        private readonly ILogger<ParserManagerService> _logger;
        private readonly INewCryptocurrencyProjectManager _projectManager;
        public void AddParser(string code, string name)
        {
            if (parsers.ContainsKey(name))
                throw new ParserAlreadyExist();
            bool successAdd = false;
            try
            {
                successAdd = parsers.TryAdd(name, createCryptoParserScheduledTask(code, name));
            }
            catch (Exception ex)
            {
                throw new ParserAddException("Parser contains error", ex);
            }
            if (!successAdd)
                throw new ParserAddException("ConcurrentDictionary.TryAdd() return false");
            _logger.LogInformation($"Parser [{name}] added");
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
            _logger.LogInformation($"Parser [{name}] removed");
        }

        private CryptoParserScheduledTask createCryptoParserScheduledTask(string code, string name)
        {
            var cryptoTask = new CryptoParserScheduledTask();
            cryptoTask.CancellationTokenSource = new CancellationTokenSource();
            cryptoTask.CryptoParser = CodeCompiler.CompileCodeAndGetObject<CryptoParserAbstract>(code);

            cryptoTask.PeriodicTask = new Task(async _ =>
            {
                var _name = name;
                try
                {
                    var _parser = cryptoTask.CryptoParser;
                    TimeSpan interval =
                        TimeSpan.FromSeconds((double)_parser.ParserConfig.RequestRateType /
                        _parser.ParserConfig.RequestsRate);

                    PeriodicTimer timer = new PeriodicTimer(interval);

                    _logger.LogDebug($"[{_name}] Task started");

                    var _localProjectsRepository = _parser.GetCryptocurrencyList();
                    await timer.WaitForNextTickAsync();
                    while (
                    !cryptoTask.CancellationTokenSource.IsCancellationRequested &&
                    await timer.WaitForNextTickAsync()
                    )
                    {
                        try
                        {
                            _logger.LogDebug($"[{_name}] requesting data");
                            var projects = _parser.GetCryptocurrencyList();

                            var newProjects = CheckProjects(projects, _localProjectsRepository);
                            
                            if (_parser.ParserConfig.MultiQueryInfoSupport)
                            {
                                await timer.WaitForNextTickAsync();
                                var infos = _parser.GetCryptocurrenciesInfo(newProjects.Select(x=>x.ParamToSearchInfo).ToArray());
                                foreach (var info in infos)
                                {
                                    newProjects.First(x=>x.ParamToSearchInfo == info.ParamToSearchInfo).CryptocurrencyInfo = info.CryptocurrencyInfo;
                                }
                            }
                            else
                            {
                                foreach (var newProject in newProjects)
                                {

                                    //if (newProject.CryptocurrencyInfo == new CryptocurrencyInfo() || newProject.CryptocurrencyInfo == null)
                                    //{
                                    //    await timer.WaitForNextTickAsync();
                                    //    _logger.LogDebug($"[{_name}] requesting additional data for {newProject.Name} project");
                                    //    newProject.CryptocurrencyInfo =
                                    //        _parser.GetCryptocurrencyInfo(newProject.ParamToSearchInfo);
                                    //}

                                    var info = _parser.GetCryptocurrencyInfo(newProject.ParamToSearchInfo, newProject.CryptocurrencyInfo ?? new CryptocurrencyInfo());

                                    if (info != null)
                                    {
                                        await timer.WaitForNextTickAsync();
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(_parser.ParserConfig.PrefixUrl))
                                newProjects.Select(x => x.ProjectUrl = x.ProjectUrl.Insert(0, _parser.ParserConfig.PrefixUrl));

                            _localProjectsRepository = projects;
                            _projectManager.AddNewProjects(_parser.CryptocurrencyExchangeUrl, newProjects);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(new ParserRuntimeException(ex), $"Parser error [{_name}]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[{_name}]");
                }
                _logger.LogWarning($"[{_name}] Task stopped");
            }, cryptoTask.CancellationTokenSource);
            cryptoTask.PeriodicTask.Start();
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
            INewCryptocurrencyProjectManager projectManager
            )
        {
            _logger = logger;
            _projectManager = projectManager;
            parsers = new ConcurrentDictionary<string, CryptoParserScheduledTask>();
        }
    }
}