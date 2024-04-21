using Shared.Integration.Models.Dtos;

namespace WS.Core.Interfaces.IntegrationServices;

public interface IWarningSentenceIntegrationService
{
    Task<List<SharedWarningSentenceDto>> GetAllWarningSentencesAsync();
}