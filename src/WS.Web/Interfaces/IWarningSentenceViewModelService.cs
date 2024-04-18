using WS.Web.ViewModels.WarningSentence;

namespace WS.Web.Interfaces;

public interface IWarningSentenceViewModelService
{
    public Task<List<WarningSentenceViewModel>> GetWarningSentenceViewModelsAsync();
    public Task<WarningSentenceViewModel> GetWarningSentenceViewModel(int id);
}