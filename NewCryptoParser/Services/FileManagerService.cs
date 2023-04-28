using NewCryptoParser.Abstract;
using NewCryptoParser.Exceptions;

namespace NewCryptoParser.Services
{
    public class FileManagerService : BackgroundService
    {
        private readonly ILogger<FileManagerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private string parsersFolderPath =>
            Environment.CurrentDirectory +
            Path.DirectorySeparatorChar +
            "Parsers";

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(parsersFolderPath).Where(file=>file.EndsWith(".cs"));
            foreach (var file in files)
            {
                try
                {
                    var _parserManager = _getParserManager();
                    using StreamReader sr = new StreamReader(file);
                    _parserManager.AddParser(await sr.ReadToEndAsync(), Path.GetFileNameWithoutExtension(file));
                }
                catch (ParserAlreadyExist)
                {
                    _logger.LogWarning($"File [{file}] added. Parser exist");
                }
                catch (CompilerException ex)
                {
                    _logger.LogError(ex, $"Compilation error occurred during auto-add file [{file}]");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred during auto-add file [{file}]");
                }
            }
            await Task.CompletedTask;
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
                    var _parserManager = _getParserManager();
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
                    var _parserManager = _getParserManager();
                    _parserManager.RemoveParser(Path.GetFileNameWithoutExtension(e.FullPath));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, 
                        $"An error occurred while uninstalling the parser due to deleting file [{e.FullPath}].");
                }
            };
            _watcher.Changed += (o, e) => 
            {
                _logger.LogDebug($"FileSystemWatcher.Changed [{e.FullPath}]");
                try
                {
                    var _parserManager = _getParserManager();
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

        private IParserManager? _getParserManager()
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetService<IParserManager>();
        }

        public FileManagerService(ILogger<FileManagerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
    }
}
