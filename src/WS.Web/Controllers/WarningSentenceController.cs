using Microsoft.AspNetCore.Mvc;
using WS.Core.Interfaces.DomainServices;
using WS.Web.Interfaces;

namespace WS.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarningSentenceController : ControllerBase
{
    private readonly IWarningSentenceViewModelService _warningSentenceViewModelService;
    private readonly IWarningSentenceService _warningSentenceService;

    public WarningSentenceController(IWarningSentenceViewModelService warningSentenceViewModelService, IWarningSentenceService warningSentenceService)
    {
        _warningSentenceViewModelService = warningSentenceViewModelService;
        _warningSentenceService = warningSentenceService;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllWarningSentences()
    {
        var warningSentences = await _warningSentenceViewModelService.GetWarningSentenceViewModelsAsync();
        return Ok(warningSentences);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWarningSentence(int id)
    {
        var warningSentence = await _warningSentenceViewModelService.GetWarningSentenceViewModel(id);
        return Ok(warningSentence);
    }
}