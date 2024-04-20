using Shared.Integration.Models.Dtos;

namespace WS.Core.Interfaces.DomainServices;

public interface IWarningSentenceIntegrationService
{
    Task<List<SharedWarningSentenceDto>> GetAllWarningSentencesAsync();
}