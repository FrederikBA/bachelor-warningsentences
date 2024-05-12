using TechTalk.SpecFlow;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WS.Infrastructure.Data;
using WS.Test.Factories;
using WS.Test.Helpers;

namespace WS.Test.Tests.ApiTests.Steps;

[Binding]
public class CreateWarningSentenceStep : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;

    public CreateWarningSentenceStep(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Given(@"an authenticated user is logged into the system")]
    public void GivenAnAuthenticatedUserIsLoggedIntoTheSystem()
    {
        _client = _factory.CreateClient();

        //Mock JWT token
        _client.DefaultRequestHeaders.Add
        (
            "Authorization",
            "Bearer " + JwtTokenHelper.GetAuthToken()
        );
    }

    [When(@"the user creates a new warning sentence")]
    public async Task WhenTheUserCreatesANewWarningSentence()
    {
        _response = await _client.PostAsJsonAsync
        (
            "/api/WarningSentence/add",
            WarningSentenceTestHelper.GetTestWarningSentenceDto()
        );
    }

    [Then(@"the system should store the warning sentence in the database")]
    public async Task ThenTheSystemShouldStoreTheWarningSentenceInTheDatabase()
    {
        //Assert that the response status code is OK
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //Assert that a warning sentence was created in the database
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WarningSentenceContext>();
        var ws = await context.WarningSentences!.FirstOrDefaultAsync();
        ws.Should().NotBeNull();
    }
}