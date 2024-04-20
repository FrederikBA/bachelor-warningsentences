using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WS.Core.Interfaces.DomainServices;

namespace WS.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "IntegrationPolicy")]
public class WarningSentenceIntegrationController : ControllerBase
{
    private readonly IWarningSentenceIntegrationService _warningSentenceIntegrationService;

    public WarningSentenceIntegrationController(IWarningSentenceIntegrationService warningSentenceIntegrationService)
    {
        _warningSentenceIntegrationService = warningSentenceIntegrationService;
    }

    [HttpGet("warningsentences")]
    public async Task<IActionResult> GetAllWarningSentences()
    {
        var warningSentences = await _warningSentenceIntegrationService.GetAllWarningSentencesAsync();
        return Ok(warningSentences);
    }
}