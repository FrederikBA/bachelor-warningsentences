using Ardalis.Specification;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Integration.Models.Dtos;
using WS.Core.Entities.WSAggregate;
using WS.Core.Exceptions;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Integration;
using WS.Core.Interfaces.Repositories;
using WS.Core.Models.Dtos;
using WS.Core.Services.DomainServices;
using WS.Test.Helpers;

namespace WS.Test.UnitTests;

public class WarningSentenceUnitTests
{
    private readonly IWarningSentenceService _warningSentenceService;
    private readonly Mock<IReadRepository<WarningSentence>> _warningSentenceReadRepositoryMock = new();
    private readonly Mock<IRepository<WarningSentence>> _warningSentenceRepositoryMock = new();
    private readonly Mock<ISyncProducer> _mockKafkaProducer = new();
    private readonly Mock<ILogger<WarningSentenceService>> _loggerMock = new();
    private readonly Mock<IProductHttpService> _productHttpServiceMock = new();

    public WarningSentenceUnitTests()
    {
        _warningSentenceService = new WarningSentenceService
        (
            _warningSentenceReadRepositoryMock.Object,
            _warningSentenceRepositoryMock.Object,
            _mockKafkaProducer.Object,
            _loggerMock.Object,
            _productHttpServiceMock.Object
        );
    }

    [Fact]
    public async Task GetWarningSentencesAsync_ReturnsListOfWarningSentences()
    {
        //Arrange
        var testWarningSentences = WarningSentenceTestHelper.GetTestWarningSentences();

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.ListAsync(It.IsAny<ISpecification<WarningSentence>>(), new CancellationToken()))
            .ReturnsAsync(testWarningSentences);

        //Act
        var result = await _warningSentenceService.GetAllWarningSentencesAsync();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); //Expecting 2 warning sentences
    }


    [Fact]
    public async Task GetWarningSentencesAsync_ThrowsWarningSentencesNotFoundException()
    {
        //Arrange
        _warningSentenceReadRepositoryMock.Setup(x =>
                x.ListAsync(It.IsAny<ISpecification<WarningSentence>>(), new CancellationToken()))
            .ReturnsAsync(new List<WarningSentence>());

        //Act
        var exception =
            await Assert.ThrowsAsync<WarningSentencesNotFoundException>(() =>
                _warningSentenceService.GetAllWarningSentencesAsync());

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task GetWarningSentenceByIdAsync_ReturnsWarningSentence()
    {
        //Arrange
        var testWarningSentences = WarningSentenceTestHelper.GetTestWarningSentences();
        var testWarningSentence = testWarningSentences.First();

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.FirstOrDefaultAsync(It.IsAny<Specification<WarningSentence>>(), new CancellationToken()))
            .ReturnsAsync(testWarningSentence);

        //Act
        var result = await _warningSentenceService.GetWarningSentenceByIdAsync(testWarningSentence.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(testWarningSentence.Id, result.Id);
    }

    [Fact]
    public async Task GetWarningSentenceByIdAsync_ThrowsWarningSentenceNotFoundException()
    {
        //Arrange
        var testWarningSentences = WarningSentenceTestHelper.GetTestWarningSentences();
        const int fakeId = 99;

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.ListAsync(It.IsAny<ISpecification<WarningSentence>>(), new CancellationToken()))
            .ReturnsAsync(testWarningSentences);

        //Act
        var exception =
            await Assert.ThrowsAsync<WarningSentenceNotFoundException>(() =>
                _warningSentenceService.GetWarningSentenceByIdAsync(fakeId));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task AddWarningSentenceAsync_ReturnsWarningSentence()
    {
        //Arrange
        var testWarningSentenceDto = WarningSentenceTestHelper.GetTestWarningSentenceDto();
        var testWarningSentence = WarningSentenceTestHelper.GetTestWarningSentences().First();

        _warningSentenceRepositoryMock.Setup(x =>
                x.AddAsync(It.IsAny<WarningSentence>(), new CancellationToken()))
            .ReturnsAsync(testWarningSentence);

        //Act
        var result = await _warningSentenceService.AddWarningSentenceAsync(testWarningSentenceDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(testWarningSentence.Code, result.Code);
    }

    [Fact]
    public async Task CloneWarningSentenceAsync_ReturnsWarningSentence()
    {
        //Arrange
        var testWarningSentence = WarningSentenceTestHelper.GetTestWarningSentences().First();
        var testWarningSentences = new List<WarningSentence> { testWarningSentence };
        var idsToClone = new List<int> { testWarningSentence.Id };

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.ListAsync(new CancellationToken()))
            .ReturnsAsync(new List<WarningSentence> { testWarningSentence });

        _warningSentenceRepositoryMock.Setup(x =>
                x.AddRangeAsync(It.IsAny<List<WarningSentence>>(), new CancellationToken()))
            .ReturnsAsync(testWarningSentences);

        //Act
        var result = await _warningSentenceService.CloneWarningSentenceAsync(idsToClone);
        var warningSentences = result.ToList();

        //Assert
        Assert.NotNull(result);
        Assert.Single(warningSentences);
    }

    [Fact]
    public async Task CloneWarningSentenceAsync_ThrowsWarningSentenceNotFoundException()
    {
        //Arrange
        _warningSentenceReadRepositoryMock.Setup(x =>
                x.ListAsync(new CancellationToken()))
            .ReturnsAsync(new List<WarningSentence> { });

        //Act
        var exception = await Assert.ThrowsAsync<WarningSentencesNotFoundException>(() =>
            _warningSentenceService.CloneWarningSentenceAsync(new List<int> { 0 }));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateWarningSentenceAsync_ShouldReturnWarningSentence()
    {
        //Arrange
        var testWarningSentence = WarningSentenceTestHelper.GetTestWarningSentences().First();
        var testWarningSentenceDto = WarningSentenceTestHelper.GetTestWarningSentenceDto();

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.GetByIdAsync(testWarningSentence.Id, new CancellationToken()))
            .ReturnsAsync(testWarningSentence);

        //Act
        var result =
            await _warningSentenceService.UpdateWarningSentenceAsync(testWarningSentence.Id, testWarningSentenceDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(testWarningSentence.Id, result.Id);
    }

    [Fact]
    public async Task UpdateWarningSentenceAsync_ShouldThrowWarningSentenceNotFoundException()
    {
        //Arrange
        _warningSentenceReadRepositoryMock.Setup(x =>
                x.GetByIdAsync(0, new CancellationToken()))
            .ReturnsAsync((WarningSentence)null!);

        //Act
        var exception = await Assert.ThrowsAsync<WarningSentenceNotFoundException>(() =>
            _warningSentenceService.UpdateWarningSentenceAsync(0, new WarningSentenceDto()));

        //Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task DeleteWarningSentenceAsync_ShouldReturnTrue()
    {
        //Arrange
        var testWarningSentences = WarningSentenceTestHelper.GetTestWarningSentences();

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.GetByIdAsync(testWarningSentences.First().Id, new CancellationToken()))
            .ReturnsAsync(testWarningSentences.First());

        _productHttpServiceMock.Setup(x => x.GetInUseWarningSentences())
            .ReturnsAsync(new SharedProductWsDto { WarningSentenceIds = new List<int> { new() } });
        
        //Act
        var result = await _warningSentenceService.DeleteWarningSentenceAsync(testWarningSentences.First().Id);

        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task DeleteWarningSentenceAsync_ShouldThrowWarningSentenceInUseException()
    {
        //Arrange
        var testWarningSentences = WarningSentenceTestHelper.GetTestWarningSentences();

        _warningSentenceReadRepositoryMock.Setup(x =>
                x.GetByIdAsync(testWarningSentences.First().Id, new CancellationToken()))
            .ReturnsAsync(testWarningSentences.First());

        _productHttpServiceMock.Setup(x => x.GetInUseWarningSentences())
            .ReturnsAsync(new SharedProductWsDto { WarningSentenceIds = new List<int> { testWarningSentences.First().Id } });
        
        //Act
        var exception = await Assert.ThrowsAsync<WarningSentenceIsInUseException>(() =>
            _warningSentenceService.DeleteWarningSentenceAsync(testWarningSentences.First().Id));

        //Assert
        Assert.NotNull(exception);
    }
}