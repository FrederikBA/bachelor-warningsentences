using WS.Core.Entities.WSAggregate;
using WS.Core.Models.Dtos;

namespace WS.Core.Interfaces.DomainServices;

public interface IWarningSentenceService
{
    Task<List<WarningSentence>> GetAllWarningSentencesAsync();
    Task<WarningSentence> GetWarningSentenceByIdAsync(int id);
    Task<WarningSentence> AddWarningSentenceAsync(WarningSentenceDto warningSentenceDto);
}