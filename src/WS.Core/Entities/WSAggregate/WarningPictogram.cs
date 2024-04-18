using Shared.Integration.Models;

namespace WS.Core.Entities.WSAggregate;

public class WarningPictogram : BaseEntity
{
    public string? Code { get; set; }
    public string? Text { get; set; }
    public string? Pictogram { get; set; }
    public string? Extension { get; set; }
    public int Priority { get; set; }
    public List<WarningSentence>? WarningSentences { get; set; }
}