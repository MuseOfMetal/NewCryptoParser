using Microsoft.EntityFrameworkCore;
using NewCryptoParser.Services;
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
    builder.Services.AddParserManager();
    builder.Services.AddNewCryptocurrencyProjectManager();
    builder.Services.AddHostedService<FileManagerService>();
    builder.Logging.ClearProviders();
    builder.Services.AddDbContext<NewCryptocurrencyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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