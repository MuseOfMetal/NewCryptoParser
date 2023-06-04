using Microsoft.AspNetCore.Mvc;
using NewCryptoParser.Abstract;

namespace NewCryptoParser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptocurrencyController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly INewCryptocurrencyProjectManager _projectManager;

        public CryptocurrencyController(ILogger<WeatherForecastController> logger, INewCryptocurrencyProjectManager projectManager)
        {
            _projectManager = projectManager;
            _logger = logger;
        }

        [HttpGet("GetLatest")]
        public IActionResult GetLatest(int count = 1)
        {
            if (count < 1)
                return UnprocessableEntity("Count must be 1 or greater");
            if (count == 1)
                return Ok(_projectManager.GetProjects().Last());
            return Ok(_projectManager.GetProjects().TakeLast(count));
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            if (id < 1)
                return UnprocessableEntity();
            var project = _projectManager.GetProjects().FirstOrDefault(x => x.Id == id);
            if (project == null)
                return NotFound();
            return Ok(project);
        }
    }
}
