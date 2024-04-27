using System.Text.Json;
using Shared.Integration.Authorization;
using Shared.Integration.Configuration;
using Shared.Integration.Models.Dtos;
using WS.Core.Interfaces.Integration;

namespace WS.Core.Services.IntegrationServices;

public class ProductHttpService : IProductHttpService
{
    private readonly HttpClient _httpClient;

    public ProductHttpService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization",
            "Bearer " + IntegrationAuthService.GetIntegrationToken());
    }

    public async Task<SharedProductWsDto> GetInUseWarningSentences()
    {
        var response = await _httpClient.GetAsync(Config.IntegrationEndpoints.ActiveWarningSentencesIntegration);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<SharedProductWsDto>(content)!;
    }
}