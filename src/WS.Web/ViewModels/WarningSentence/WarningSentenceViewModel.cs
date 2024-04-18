namespace WS.Web.ViewModels.WarningSentence;

public class WarningSentenceViewModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Text { get; set; }
    public WarningCategoryViewModel? WarningCategory { get; set; }
    public WarningSignalWordViewModel? WarningSignalWord { get; set; }
    public WarningPictogramViewModel? WarningPictogram { get; set; }
}