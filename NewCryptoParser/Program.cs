using Microsoft.EntityFrameworkCore;
using NewCryptoParser.Abstract;
using NewCryptoParser.Services;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    //builder.Services.AddParserManager();
    //builder.Services.AddSingleton<IFileManager, FileManagerService>();
    builder.Services.AddScoped<INewCryptocurrencyProjectManager, NewCryptocurrencyProjectManagerService>();
    builder.Services.AddScoped<IParserManager, ParserManagerService>();
    builder.Services.AddHostedService<FileManagerService>();
    
    builder.Logging.ClearProviders();
    builder.Services.AddDbContext<DbService>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Host.UseNLog();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
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
    Console.WriteLine("Hello");
    LogManager.Shutdown();
}