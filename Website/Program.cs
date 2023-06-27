namespace Website;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddHttpClient("NewCryptoParser", httpClient =>
        {
            httpClient.BaseAddress = new(builder.Configuration.GetSection("ApiAddresss").Get<string>());
        });
        builder.Services.AddScoped<ParserInfoService>();
        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();
        app.Run();
    }
}