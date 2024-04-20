using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WS.Web.Interfaces;

namespace WS.Web.Controllers.IntegrationControllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "IntegrationPolicy")]
public class WarningSentenceIntegrationController : ControllerBase
{
    private readonly IWarningSentenceViewModelService _warningSentenceViewModelService;

    public WarningSentenceIntegrationController(IWarningSentenceViewModelService warningSentenceViewModelService)
    {
        _warningSentenceViewModelService = warningSentenceViewModelService;
    }
    
    [HttpGet("warningsentences")]
    public async Task<IActionResult> GetAllWarningSentences()
    {
        var warningSentences = await _warningSentenceViewModelService.GetWarningSentenceViewModelsAsync();
        return Ok(warningSentences);
    }
}