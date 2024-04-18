using Shared.Integration.Models;

namespace WS.Core.Entities.WSAggregate;

public class WarningSignalWord : BaseEntity
{
    public string? SignalWordText { get; set; }
    public int Priority { get; set; }
    public List<WarningSentence>? WarningSentences { get; set; }
}