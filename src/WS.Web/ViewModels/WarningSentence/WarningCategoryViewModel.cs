namespace WS.Web.ViewModels.WarningSentence;

public class WarningCategoryViewModel
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public WarningTypeViewModel? WarningType { get; set; }
}