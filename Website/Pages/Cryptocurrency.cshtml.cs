using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using Website.Models;

namespace Website.Pages
{
    public class CryptocurrencyModel : PageModel
    {
        private readonly ILogger<CryptocurrencyModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ParserInfoService _parserInfoService;

        public NewCryptocurrencyProject? Project { get; set; }

        public CryptocurrencyModel(ILogger<CryptocurrencyModel> logger, IHttpClientFactory httpClientFactory, ParserInfoService parserInfoService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _parserInfoService = parserInfoService;
        }

        public async Task OnGet(int id)
        {
            var client = _httpClientFactory.CreateClient("NewCryptoParser");

            try
            {
                Project = JsonConvert
                    .DeserializeObject<NewCryptocurrencyProject>(await client.GetStringAsync($"cryptocurrency/GetById?id={id}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
            }
        }

        public string? GetName(string exchangeUrl)
        {
            return _parserInfoService.GetName(exchangeUrl);
        }
        public string? GetImageUrl(string exchangeUrl)
        {
            return _parserInfoService.GetImageUrl(exchangeUrl);
        }
    }
}