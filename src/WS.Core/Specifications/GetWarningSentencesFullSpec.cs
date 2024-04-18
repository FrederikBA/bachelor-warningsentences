using Ardalis.Specification;
using WS.Core.Entities.WSAggregate;

namespace WS.Core.Specifications;


public sealed class GetWarningSentencesFullSpec : Specification<WarningSentence>
{
    public GetWarningSentencesFullSpec()
    {
        Query
            .Include(warningSentence => warningSentence.WarningCategory)
            .ThenInclude(warningCategory => warningCategory!.WarningType)
            .Include(warningSentence => warningSentence.WarningPictogram)
            .Include(warningSentence => warningSentence.WarningSignalWord);
    }
}