using Shared.Integration.Models;

namespace WS.Core.Entities.WSAggregate;

public class WarningCategory : BaseEntity
{
    public string? Text { get; set; }
    public int WarningTypeId { get; set; }
    public int SortOrder { get; set; }
    public WarningType? WarningType { get; set; }
    public List<WarningSentence>? WarningSentences { get; set; }
}