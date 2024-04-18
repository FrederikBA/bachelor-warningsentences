using Ardalis.Specification;
using WS.Core.Entities.WSAggregate;

namespace WS.Core.Specifications;

public sealed class GetWarningSentenceByIdFullSpec : Specification<WarningSentence>
{
    public GetWarningSentenceByIdFullSpec(int id)
    {
        Query
            .Where(warningSentence => warningSentence.Id == id)
            .Include(warningSentence => warningSentence.WarningCategory)
            .ThenInclude(warningCategory => warningCategory!.WarningType)
            .Include(warningSentence => warningSentence.WarningPictogram)
            .Include(warningSentence => warningSentence.WarningSignalWord);
    }
}