using Shared.Integration.Models.Dtos;

namespace WS.Core.Interfaces.Integration;

public interface IWarningSentenceIntegrationService
{
    Task<List<SharedWarningSentenceDto>> GetAllWarningSentencesAsync();
}