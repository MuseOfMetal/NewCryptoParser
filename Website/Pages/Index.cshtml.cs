using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Website.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ParserInfoService _parserInfoService;
        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, ParserInfoService parserInfoService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _parserInfoService = parserInfoService;
        }

        public List<NewCryptocurrencyProject>? Projects { get; set; }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("NewCryptoParser");

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    Projects =
                        JsonConvert
                        .DeserializeObject<List<NewCryptocurrencyProject>>(await client.GetStringAsync($"cryptocurrency/search?query={search}"));
                }
                else
                {
                    Projects =
                        JsonConvert
                        .DeserializeObject<List<NewCryptocurrencyProject>>(await client.GetStringAsync("cryptocurrency/getLatest?count=100"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OnGet error");
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




        [BindProperty(SupportsGet = true)]
        public string? search { get; set; }
        public SelectList? Cryptocurrencies { get; set; }
    }
}