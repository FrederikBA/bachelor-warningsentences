using WS.Core.Entities.WSAggregate;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Repositories;

namespace WS.Core.Services;

public class WarningSentenceService : IWarningSentenceService
{
    private readonly IReadRepository<WarningSentence> _warningSentenceReadRepository;
    private readonly IRepository<WarningSentence> _warningSentenceRepository;

    public WarningSentenceService(IReadRepository<WarningSentence> warningSentenceReadRepository, IRepository<WarningSentence> warningSentenceRepository)
    {
        _warningSentenceReadRepository = warningSentenceReadRepository;
        _warningSentenceRepository = warningSentenceRepository;
    }

    public Task<List<WarningSentence>> GetAllWarningSentencesAsync()
    {
        throw new NotImplementedException();
    }
}