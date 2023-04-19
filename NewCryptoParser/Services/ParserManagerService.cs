using CryptoParserSdk;
using NewCryptoParser.Abstract;
using System.Collections.Concurrent;
using NewCryptoParser.Exceptions;
using System.Diagnostics;
using NewCryptoParser.Models;
using System.Threading;
using System.Security.Cryptography;
using CryptoParserSdk.Models;

namespace NewCryptoParser.Services
{

    public class ParserManagerService : IParserManager
    {
        private ConcurrentDictionary<string, CryptoParserScheduledTask> parsers;
        private object lockObj;
        private readonly ILogger<ParserManagerService> _logger;
        private readonly IParserManager _parserManager;

        public void AddParser(string code, string name)
        {
            parsers.TryAdd(name, createCryptoParserScheduledTask(code, name));

            static List<ParsingResult> CheckProjects(List<ParsingResult> parsingResults, List<ParsingResult> repository)
            {
                var newProjects = new List<ParsingResult>();
                foreach (var project in parsingResults)
                {
                    if (repository.FirstOrDefault(p => p.ProjectUrl == project.ProjectUrl) == null)
                    {
                        newProjects.Add(project);
                    }
                }
                return newProjects;
            }
        }

        public ICryptoParser GetParser(string name)
        {
            parsers.TryGetValue(name, out var parser);
            return parser.CryptoParser;
        }

        public void RemoveParser(string name)
        {
            parsers.TryRemove(name, out var parser);
            parser.Dispose();
        }

        public void UpdateParser(string code, string name)
        {
            parsers.TryGetValue(name, out var parser);
            parser.Dispose();

            parsers.TryUpdate(name, createCryptoParserScheduledTask(code, name), parser);
        }

        private CryptoParserScheduledTask createCryptoParserScheduledTask(string code, string name)
        {
            var cryptoTask = new CryptoParserScheduledTask();
            cryptoTask.CancellationTokenSource = new CancellationTokenSource();
            cryptoTask.CryptoParser = CodeCompiler.CompileCodeAndGetObject<ICryptoParser>(code); ;
            cryptoTask.PeriodicTask = new Task(async _ =>
            {
                var parser = cryptoTask.CryptoParser;
                var _localProjectsRepository = parser.GetCryptocurrencyList();
                TimeSpan interval = TimeSpan.FromMilliseconds((int)parser.ParserConfig.RequestRateType / parser.ParserConfig.RequestsRate);
                PeriodicTimer timer = new PeriodicTimer(interval);
                while (!cryptoTask.CancellationTokenSource.IsCancellationRequested && await timer.WaitForNextTickAsync())
                {
                    var projects = parser.GetCryptocurrencyList();
                    var newProjects = CheckProjects(projects, _localProjectsRepository);
                    foreach (var newProject in newProjects)
                    {
                        if (newProject.CryptocurrencyInfo == null)
                            newProject.CryptocurrencyInfo = parser.GetCryptocurrencyInfo(newProject.ParamToSearchInfo);
                    }
                    _localProjectsRepository = newProjects;

                }

            }, cryptoTask.CancellationTokenSource);
        }

    }

    //public class ParserManagerService
    //{
    //    private FileSystemWatcher watcher;
    //    private object locker;
    //    private ConcurrentDictionary<string, ICryptoParser> parsers;
    //    private Dictionary<string, CryptoParserScheduledTask> tasks;
    //    private readonly ILogger<ParserManagerService> _logger;
    //    private string parsersFolderPath =>
    //        Environment.CurrentDirectory +
    //        Path.DirectorySeparatorChar +
    //        "Parsers";
    //    private string disabledParsersFolderPath =>
    //        parsersFolderPath +
    //        Path.DirectorySeparatorChar +
    //        "Disabled";
    //    public ParserManagerService(ILogger<ParserManagerService> logger)
    //    {
    //        _logger = logger;
    //        if (!Directory.Exists(parsersFolderPath))
    //            Directory.CreateDirectory(parsersFolderPath);
    //        if (!Directory.Exists(disabledParsersFolderPath))
    //            Directory.CreateDirectory(disabledParsersFolderPath);
    //        //*********************************

    //        //Initalizing fields
    //        locker = new();
    //        parsers = new();
    //        tasks = new();
    //        watcher = new FileSystemWatcher(parsersFolderPath);
    //        watcher.Created += fileCreated;
    //        watcher.Deleted += fileDeleted;
    //        //********************

    //        var files =
    //            Directory.GetFiles(parsersFolderPath)
    //            .Where(file => file.EndsWith(".cs"));


    //        foreach (var item in files)
    //        {
    //            try
    //            {
    //                _logger.LogInformation($"Loading [{item}] parser");
    //                waitForFile(item);
    //                add(item);
    //                _logger.LogInformation($"Parser [{item}] loaded successfully");
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.LogError(ex, $"Error occured while loading parser [{item}]");
    //            }
    //        }

    //        watcher.EnableRaisingEvents = true;

    //        _logger.LogInformation("Initialize complete. Parsers loaded: " + parsers.Count);

    //    }

    //    public ICollection<string> GetCryptoParsersName()
    //    {
    //        return parsers.Keys;
    //    }

    //    public ICryptoParser? GetCryptoParserByName(string name)
    //    {
    //        ICryptoParser? parser;
    //        parsers.TryGetValue(name, out parser);
    //        return parser;
    //    }

    //    #region Private
    //    private void fileDeleted(object sender, FileSystemEventArgs e)
    //    {
    //        lock (locker)
    //        {
    //            if (!e.FullPath.EndsWith(".cs"))
    //                return;
    //            _logger.LogInformation($"Unloading parser [{e.Name}]");
    //            ICryptoParser? cryptoParser;
    //            parsers.Remove(Path.GetFileNameWithoutExtension(e.FullPath), out cryptoParser);
    //            _logger.LogInformation($"Parser [{e.Name}] unloaded");
    //        }
    //    }
    //    private void fileCreated(object sender, FileSystemEventArgs e)
    //    {
    //        _logger.LogInformation($"Loading new parser [{e.Name}]");
    //        try
    //        {
    //            waitForFile(e.FullPath);
    //            lock (locker)
    //            {
    //                if (!e.FullPath.EndsWith(".cs"))
    //                    return;
    //                add(e.FullPath);
    //                _logger.LogInformation($"Parser [{e.Name}] loaded successfully");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"Error occured while loading parser [{e.Name}]");
    //        }

    //    }
    //    private void waitForFile(string filename, int timeout = 1000)
    //    {
    //        var time = Stopwatch.StartNew();
    //        while (time.ElapsedMilliseconds < timeout)
    //        {
    //            if (isFileReady(filename))
    //                return;
    //        }
    //        throw new TimeoutException($"The file [{filename}] has been unavailable for {timeout} ms");
    //    }
    //    private bool isFileReady(string filename)
    //    {
    //        // If the file can be opened for exclusive access it means that the file
    //        // is no longer locked by another process.
    //        try
    //        {
    //            using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
    //                return inputStream.Length > 0;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //    }
    //    private void add(string path)
    //    {
    //        for (int i = 1; !parsers.TryAdd(Path.GetFileNameWithoutExtension(path), CodeCompiler.CompileFileAndGetObject<ICryptoParser>(path)) && i <= 10; i++)
    //        {
    //            _logger.LogWarning($"Failed to load parser {Path.GetFileName(path)}. Attempt {i}/10");
    //            Thread.Sleep(100);
    //        }
    //        throw new TooManyAttemptsException($"Too many attempts to load parser [{Path.GetFileName(path)}]");
    //    }

    //    private void _add(string path)
    //    {
    //        var name = Path.GetFileNameWithoutExtension(path);
    //        var obj = CodeCompiler.CompileFileAndGetObject<ICryptoParser>(path);

    //        var task = new Timer(_ =>
    //        {

    //            try
    //            {
    //                var parser = obj;
    //                var _localProjectsRepository = parser.GetCryptocurrencyList();
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.LogError(ex, $"Parser [{name}] disabled. ");
    //                Disable(name);
    //                return;
    //            }

    //            //Thread sleep

    //        });

    //        throw new TooManyAttemptsException($"Too many attempts to load parser [{Path.GetFileName(path)}]");
    //    }

    //    #endregion
    //    public bool Enable()
    //    {
    //        return true;
    //    }

    //    public bool Disable(string parserName)
    //    {
    //        return true;
    //    }

    //}
}