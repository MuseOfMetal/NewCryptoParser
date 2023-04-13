using CryptoParserSdk.Models;
using NewCryptoParser.Abstract;
using NewCryptoParser.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace NewCryptoParser.Services
{
    public class NewCryptocurrencyProjectManagerService : INewCryptocurrencyProjectManager
    {
        [NotNull]
        private readonly DbService _context;
        private readonly ILogger<NewCryptocurrencyProjectManagerService> _logger;
        private Timer _timer;
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
            return _context.NewCryptocurrencyProjects.Last();
        }

        public NewCryptocurrencyProject? GetProjectById(int id)
        {
            return _context.NewCryptocurrencyProjects.Find(id);
        }

        public List<NewCryptocurrencyProject>? GetProjectsByIdRange(int startId, int endId)
        {
            throw new NotImplementedException();
        }

        public NewCryptocurrencyProjectManagerService(DbService context, ILogger<NewCryptocurrencyProjectManagerService> logger)
        {
            _context = context;
            _logger = logger;
            _buffer = new ConcurrentQueue<(string parserName, ParsingResult project)>();
            _timer = new Timer(_ =>
            {
                try
                {
                    while (_buffer.Count > 0)
                    {
                        _buffer.TryDequeue(out var result);
                        dataProccessor(result.parserName, result.project);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured while proccess new cryptocurrency project");
                }
            }, null, 0, 1000);
        }

        private void dataProccessor(string parserName, ParsingResult project)
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
            NewCryptocurrencyProjectInfo info = project.CryptocurrencyInfo as NewCryptocurrencyProjectInfo ?? new NewCryptocurrencyProjectInfo();
            info.ParserName = parserName;
            info.ProjectUrl = project.ProjectUrl;
            if (finded == null)
            {
                _context.NewCryptocurrencyProjects.Add(new NewCryptocurrencyProject()
                {
                    ParserName = parserName,
                    ProjectName = project.Name,
                    ProjectSymbol = project.Symbol,
                    Infos = new List<NewCryptocurrencyProjectInfo> { info }
                });
            }
            else
            {
                finded.Infos.Add(info);
            }
            _context.SaveChanges();
        }
    }
}
