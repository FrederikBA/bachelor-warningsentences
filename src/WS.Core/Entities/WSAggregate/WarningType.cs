using Shared.Integration.Models;

namespace WS.Core.Entities.WSAggregate;

public class WarningType : BaseEntity
{
    public string? Type { get; set; }
    public int Priority { get; set; }
    public List<WarningCategory>? WarningCategories { get; set; }
}