using WS.Core.Entities.WSAggregate;

namespace WS.Core.Interfaces.DomainServices;

public interface IWarningSentenceService
{
    Task<List<WarningSentence>> GetAllWarningSentencesAsync();
}