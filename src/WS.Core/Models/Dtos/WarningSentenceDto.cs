namespace WS.Core.Models.Dtos;

public class WarningSentenceDto
{
    public string? Code { get; set; }
    public string? Text { get; set; }
    public int WarningTypeId { get; set;}
    public int WarningCategoryId { get; set; }
    public int WarningPictogramId { get; set; }
    public int WarningSignalWordId { get; set; }
}