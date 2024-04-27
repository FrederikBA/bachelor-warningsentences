using Shared.Integration.Models.Dtos;

namespace WS.Core.Interfaces.Integration;

public interface IProductHttpService
{
    public Task<SharedProductWsDto> GetInUseWarningSentences();
}