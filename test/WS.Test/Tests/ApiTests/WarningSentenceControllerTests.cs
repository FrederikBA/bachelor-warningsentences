using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WS.Core.Models.Dtos;
using WS.Infrastructure.Data;
using WS.Test.Factories;
using WS.Test.Helpers;

namespace WS.Test.Tests.ApiTests;

public class WarningSentenceControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient? _client;

    public WarningSentenceControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AddWarningSentence_ReturnsSuccessStatusCode()
    {
        // Arrange
        _client = _factory.CreateClient();

        //Mock JWT token
        _client.DefaultRequestHeaders.Add
        (
            "Authorization",
            "Bearer " + JwtTokenHelper.GetAuthToken()
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/WarningSentence/add",
            WarningSentenceTestHelper.GetTestWarningSentenceDto());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //Check the in-memory database to see if the warning sentence was created
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WarningSentenceContext>();
        var ws = await context.WarningSentences!.FirstOrDefaultAsync();
        Assert.NotNull(ws);
    }
}