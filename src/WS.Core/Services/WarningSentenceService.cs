using WS.Core.Entities.WSAggregate;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Repositories;

namespace WS.Core.Services;

public class WarningSentenceService : IWarningSentenceService
{
    private readonly IReadRepository<WarningSentence> _warningSentenceReadRepository;

    public WarningSentenceService(IReadRepository<WarningSentence> warningSentenceReadRepository)
    {
        _warningSentenceReadRepository = warningSentenceReadRepository;
    }

    public Task<List<WarningSentence>> GetAllWarningSentencesAsync()
    {
        throw new NotImplementedException();
    }
}