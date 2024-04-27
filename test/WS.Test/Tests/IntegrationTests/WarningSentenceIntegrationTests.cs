using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using WS.Core.Entities.WSAggregate;
using WS.Core.Interfaces.Integration;
using WS.Core.Interfaces.Repositories;
using WS.Core.Services.DomainServices;
using WS.Infrastructure.Data;
using WS.Test.Drivers;
using WS.Test.Helpers;

namespace WS.Test.Tests.IntegrationTests;

public class WarningSentenceIntegrationTests : IDisposable
{
    private readonly WarningSentenceContext _context;

    public WarningSentenceIntegrationTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<WarningSentenceContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()))
            .BuildServiceProvider();

        _context = serviceProvider.GetRequiredService<WarningSentenceContext>();
    }

    [Fact]
    public async Task AddWarningSentence_ReturnsWarningSentence()
    {
        //Arrange
        var wsRepository = new EfRepository<WarningSentence>(_context);
        var syncProducerMock = new Mock<ISyncProducer>();
        var loggerMock = new Mock<ILogger<WarningSentenceService>>();
        var productHttpServiceMock = new Mock<IProductHttpService>();
        var wsService = new WarningSentenceService(null, wsRepository, syncProducerMock.Object, loggerMock.Object,
            productHttpServiceMock.Object);

        var dto = WarningSentenceTestHelper.GetTestWarningSentenceDto();

        // Seed data
        var testOrders = WarningSentenceTestHelper.GetTestWarningSentences();
        await _context.WarningSentences.AddRangeAsync(testOrders);
        await _context.SaveChangesAsync();


        //Act
        var result = await wsService.AddWarningSentenceAsync(dto);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddWarningSentence_ProducesMessage()
    {
        //Arrange
        var syncProducer = new TestProducer();
        var wsRepositoryMock = new Mock<IRepository<WarningSentence>>();
        var wsReadRepositoryMock = new Mock<IReadRepository<WarningSentence>>();
        var loggerMock = new Mock<ILogger<WarningSentenceService>>();
        var productHttpServiceMock = new Mock<IProductHttpService>();
        var wsService = new WarningSentenceService
        (
            wsReadRepositoryMock.Object,
            wsRepositoryMock.Object,
            syncProducer,
            loggerMock.Object,
            productHttpServiceMock.Object
        );

        //Mock AddAsync method
        wsRepositoryMock.Setup(x => x.AddAsync(It.IsAny<WarningSentence>(), new CancellationToken()))
            .ReturnsAsync(WarningSentenceTestHelper.GetTestWarningSentences().First());

        var dto = WarningSentenceTestHelper.GetTestWarningSentenceDto();

        //Setup kafka environment
        await TestTopicManager.CreateTopic("test-topic-ws");

        //Act
        await wsService.AddWarningSentenceAsync(dto);

        //Get kafka topic message count
        var topicMessages = await TestTopicManager.GetTopicMessages("test-topic-ws");

        //Assert
        Assert.Single(topicMessages); // We expect 1 message in the topic
    }

    public async void Dispose()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();

        //Delete kafka topic after test
        await TestTopicManager.DeleteTopic("test-topic-ws");
    }
}