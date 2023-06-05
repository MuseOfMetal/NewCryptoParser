using NewCryptoParser.Abstract;
using NewCryptoParser.Exceptions;

namespace NewCryptoParser.Services;

public class FileManagerService : BackgroundService
{
    private readonly ILogger<FileManagerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfigurationProvider _configurationProvider;
    private string parsersFolderPath =>
        Environment.CurrentDirectory +
        Path.DirectorySeparatorChar +
        "Parsers";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var files = Directory.GetFiles(parsersFolderPath).Where(file => file.EndsWith(".cs"));
        foreach (var file in files)
        {
            try
            {
                var _parserManager = _getParserManager();
                using StreamReader sr = new StreamReader(file);
                _parserManager.AddParser(await sr.ReadToEndAsync(), Path.GetFileNameWithoutExtension(file));
                _logger.LogInformation($"Parser [{file}] successfully loaded");
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
                _logger.LogError(ex, $"An error occurred during auto-add file [{file}]");
            }
        }

        _logger.LogInformation("File manager service started.");
        using var _watcher = new FileSystemWatcher(parsersFolderPath);
        if (!Directory.Exists(parsersFolderPath))
            Directory.CreateDirectory(parsersFolderPath);

        _watcher.Created += (o, e) =>
        {
            try
            {
                _logger.LogDebug($"FileSystemWatcher.Created [{e.FullPath}]");
                if (!e.FullPath.EndsWith(".cs"))
                    return;
                WaitForFile(e.FullPath);
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
                if (!e.FullPath.EndsWith(".cs"))
                    return;
                var _parserManager = _getParserManager();
                _parserManager.RemoveParser(Path.GetFileNameWithoutExtension(e.FullPath));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, 
                    $"An error occurred while uninstalling the parser due to deleting file [{e.FullPath}].");
            }
        };

        _watcher.EnableRaisingEvents = true;

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(200, stoppingToken);
        }
        _logger.LogWarning("File manager service stopped.");
        await Task.CompletedTask;
    }

    private IParserManager? _getParserManager()
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetService<IParserManager>();
    }


    private bool IsFileReady(string filename)
    {
        try
        {
            _logger.LogDebug($"Waiting for file [{filename}]");
            using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                return inputStream.Length > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private void WaitForFile(string filename)
    {
        while (!IsFileReady(filename)) { }
    }

    public FileManagerService(ILogger<FileManagerService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
}