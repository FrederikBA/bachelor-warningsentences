using Shared.Integration.Models.Dtos;
using WS.Core.Entities.WSAggregate;
using WS.Core.Exceptions;
using WS.Core.Interfaces.Integration;
using WS.Core.Interfaces.Repositories;
using WS.Core.Specifications;

namespace WS.Core.Services.IntegrationServices;

public class WarningSentenceIntegrationService : IWarningSentenceIntegrationService
{
    private readonly IReadRepository<WarningSentence> _warningSentenceReadRepository;

    public WarningSentenceIntegrationService(IReadRepository<WarningSentence> warningSentenceReadRepository)
    {
        _warningSentenceReadRepository = warningSentenceReadRepository;
    }

    public async Task<List<SharedWarningSentenceDto>> GetAllWarningSentencesAsync()
    {
        var warningSentences = await _warningSentenceReadRepository.ListAsync(new GetWarningSentencesFullSpec());

        if (warningSentences == null || warningSentences.Count == 0)
        {
            throw new WarningSentencesNotFoundException();
        }
        
        var sharedWarningSentences = warningSentences.Select(warningSentence => new SharedWarningSentenceDto
        {
            WarningSentenceId = warningSentence.Id,
            WarningSentenceCode = warningSentence.Code,
        }).ToList();

        return sharedWarningSentences;
    }
}