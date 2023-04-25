using Microsoft.Extensions.Options;
using NewCryptoParser.Abstract;
using NewCryptoParser.Exceptions;

namespace NewCryptoParser.Services
{
    //public class FileManagerService : IFileManager
    //{
    //    private static FileSystemWatcher _watcher;
    //    private readonly IParserManager _parserManager;
    //    private string parsersFolderPath =>
    //        Environment.CurrentDirectory +
    //        Path.DirectorySeparatorChar +
    //        "Parsers";
    //    private void _file_changed(object sender, FileSystemEventArgs e)
    //    {
    //        using StreamReader sr = new StreamReader(e.FullPath);
    //        _parserManager.UpdateParser(sr.ReadToEnd(), Path.GetFileNameWithoutExtension(e.FullPath));
    //    }

    //    private void _file_deleted(object sender, FileSystemEventArgs e)
    //    {
    //        _parserManager.RemoveParser(Path.GetFileNameWithoutExtension(e.FullPath));
    //    }

    //    private void _file_created(object sender, FileSystemEventArgs e)
    //    {
    //        using StreamReader sr = new StreamReader(e.FullPath);
    //        _parserManager.AddParser(sr.ReadToEnd(), Path.GetFileNameWithoutExtension(e.FullPath));
    //    }

    //    public string GetWorkPath()
    //    {
    //        return parsersFolderPath;
    //    }

    //    public string[] GetFiles()
    //    {
    //        return Directory.GetFiles(parsersFolderPath);
    //    }

    //    public void AddFile()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RemoveFile()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public FileManagerService(IParserManager parserManager)
    //    {
    //        if (!Directory.Exists(parsersFolderPath))
    //            Directory.CreateDirectory(parsersFolderPath);
    //        _parserManager = parserManager;
    //        _watcher = new FileSystemWatcher(parsersFolderPath);
    //        _watcher.Created += _file_created;
    //        _watcher.Deleted += _file_deleted;
    //        _watcher.Changed += _file_changed;
    //        _watcher.EnableRaisingEvents = true;
    //    }
    //}

    public class FileManagerService : BackgroundService
    {
        private readonly ILogger<FileManagerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private string parsersFolderPath =>
            Environment.CurrentDirectory +
            Path.DirectorySeparatorChar +
            "Parsers";
        private string path => _configurationProvider.Get

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task started.");
            using var _watcher = new FileSystemWatcher(parsersFolderPath);
            if (!Directory.Exists(parsersFolderPath))
                Directory.CreateDirectory(parsersFolderPath);

            //_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

            _watcher.Created += (o, e) =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var _parserManager = scope.ServiceProvider.GetService<IParserManager>();
                    using StreamReader sr = new StreamReader(e.FullPath);
                    _parserManager.AddParser(sr.ReadToEnd(), Path.GetFileNameWithoutExtension(e.FullPath));
                }
                catch (CompilerException ex)
                {
                    _logger.LogError(ex, $"Error compiling [{e.FullPath}] file.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding a parser from file [{e.FullPath}].");
                }
            };
            _watcher.Deleted += (o, e) => 
            {
                _logger.LogDebug($"FileSystemWatcher.Deleted [{e.FullPath}]");
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var _parserManager = scope.ServiceProvider.GetService<IParserManager>();
                    _parserManager.RemoveParser(Path.GetFileNameWithoutExtension(e.FullPath));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while uninstalling the parser due to deleting file [{e.FullPath}].");
                }
            };
            _watcher.Changed += (o, e) => 
            {
                _logger.LogDebug($"FileSystemWatcher.Changed [{e.FullPath}]");
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var _parserManager = scope.ServiceProvider.GetService<IParserManager>();
                    using StreamReader sr = new StreamReader(e.FullPath);
                    _parserManager.UpdateParser(sr.ReadToEnd(), Path.GetFileNameWithoutExtension(e.FullPath));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the parser from file [{e.FullPath}].");
                    
                }
            };

            _watcher.EnableRaisingEvents = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(200, stoppingToken);
            }
            _logger.LogInformation("Task completed.");
            await Task.CompletedTask;
        }

        public FileManagerService(ILogger<FileManagerService> logger, IServiceProvider serviceProvider, Iconfig configurationProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configurationProvider = configurationProvider;
        }
    }
}
