using Microsoft.EntityFrameworkCore;
using NewCryptoParser.Abstract;
using NewCryptoParser.Services;
using Newtonsoft.Json;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<INewCryptocurrencyProjectManager, NewCryptocurrencyProjectManagerService>();
    builder.Services.AddSingleton<IParserManager, ParserManagerService>();
    builder.Services.AddHostedService<FileManagerService>();
    builder.Logging.ClearProviders();
    builder.Services.AddDbContext<DbService>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Host.UseNLog();
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }    
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}