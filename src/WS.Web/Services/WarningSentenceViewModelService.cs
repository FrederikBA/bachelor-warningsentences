using WS.Core.Interfaces.DomainServices;
using WS.Web.Interfaces;
using WS.Web.ViewModels.WarningSentence;

namespace WS.Web.Services;

public class WarningSentenceViewModelService : IWarningSentenceViewModelService
{
    private readonly IWarningSentenceService _warningSentenceService;

    public WarningSentenceViewModelService(IWarningSentenceService warningSentenceService)
    {
        _warningSentenceService = warningSentenceService;
    }

    public async Task<List<WarningSentenceViewModel>> GetWarningSentenceViewModelsAsync()
    {
        var warningSentenceEntities = await _warningSentenceService.GetAllWarningSentencesAsync();

        var warningSentenceViewModels = warningSentenceEntities.Select(warningSentence => new WarningSentenceViewModel
        {
            Id = warningSentence.Id,
            Code = warningSentence.Code,
            Text = warningSentence.Text,
            
            //Warning Category
            WarningCategory = new WarningCategoryViewModel
            {
                Id = warningSentence.WarningCategory!.Id,
                Text = warningSentence.WarningCategory!.Text,
                
                //Warning Type
                WarningType = new WarningTypeViewModel
                {
                    Type = warningSentence.WarningCategory!.WarningType!.Type,
                }
            },
            
            //Warning Pictogram
            WarningPictogram = new WarningPictogramViewModel
            {
                Id = warningSentence.WarningPictogram!.Id,
                Code = warningSentence.WarningPictogram!.Code,
                Text = warningSentence.WarningPictogram!.Text,
                Pictogram = warningSentence.WarningPictogram!.Pictogram,
                Extension = warningSentence.WarningPictogram!.Extension,
            },
            
            //Warning Signal Word
            WarningSignalWord = new WarningSignalWordViewModel
            {
                Id = warningSentence.WarningSignalWord!.Id,
                SignalWordText = warningSentence.WarningSignalWord!.SignalWordText,
            }
        }).ToList();

        return warningSentenceViewModels;
    }

    public async Task<WarningSentenceViewModel> GetWarningSentenceViewModel(int id)
    {
        var warningSentenceEntity = await _warningSentenceService.GetWarningSentenceByIdAsync(id);
        
        var warningSentenceViewModel = new WarningSentenceViewModel
        {
            Id = warningSentenceEntity.Id,
            Code = warningSentenceEntity.Code,
            Text = warningSentenceEntity.Text,
            
            //Warning Category
            WarningCategory = new WarningCategoryViewModel
            {
                Id = warningSentenceEntity.WarningCategory!.Id,
                Text = warningSentenceEntity.WarningCategory!.Text,
                
                //Warning Type
                WarningType = new WarningTypeViewModel
                {
                    Type = warningSentenceEntity.WarningCategory!.WarningType!.Type,
                }
            },
            
            //Warning Pictogram
            WarningPictogram = new WarningPictogramViewModel
            {
                Id = warningSentenceEntity.WarningPictogram!.Id,
                Code = warningSentenceEntity.WarningPictogram!.Code,
                Text = warningSentenceEntity.WarningPictogram!.Text,
                Pictogram = warningSentenceEntity.WarningPictogram!.Pictogram,
                Extension = warningSentenceEntity.WarningPictogram!.Extension,
            },
            
            //Warning Signal Word
            WarningSignalWord = new WarningSignalWordViewModel
            {
                Id = warningSentenceEntity.WarningSignalWord!.Id,
                SignalWordText = warningSentenceEntity.WarningSignalWord!.SignalWordText,
            }
        };
        
        return warningSentenceViewModel;
    }
}