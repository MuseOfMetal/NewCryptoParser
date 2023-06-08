using Microsoft.AspNetCore.Mvc;
using NewCryptoParser.Abstract;

namespace NewCryptoParser.Controllers;

[ApiController]
[Route("[controller]")]
public class CryptocurrencyController : ControllerBase
{
    private readonly ILogger<CryptocurrencyController> _logger;
    private readonly INewCryptocurrencyProjectManager _projectManager;

    public CryptocurrencyController(ILogger<CryptocurrencyController> logger, INewCryptocurrencyProjectManager projectManager)
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
        var _plist = _projectManager.GetProjects().ToList();
        _plist.Reverse();
        if (_plist.Count() <= count)
            return Ok(_plist);
        return Ok(_plist.Take(count));

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

    [HttpGet("Search")]
    public IActionResult Search(string query)
    {
        if (string.IsNullOrEmpty(query))
            return UnprocessableEntity("query parameter cannot be empty");
        var projects = _projectManager.GetProjects().ToList();
        projects.Reverse();
        var selectedProjects = projects.Where(x=>x.ProjectName.ToLower().Contains(query.ToLower()) || x.ProjectSymbol.ToLower().Contains(query.ToLower()));
        return Ok(selectedProjects);
    }
}