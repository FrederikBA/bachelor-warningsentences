using Ardalis.Specification;
using Moq;
using WS.Core.Entities.WSAggregate;
using WS.Core.Exceptions;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Repositories;
using WS.Core.Services;
using WS.Test.Helpers;

namespace WS.Test.UnitTests;

public class WarningSentenceUnitTests
{
    private readonly IWarningSentenceService _warningSentenceService;
    private readonly Mock<IReadRepository<WarningSentence>> _warningSentenceReadRepositoryMock = new();
    private readonly Mock<IRepository<WarningSentence>> _warningSentenceRepositoryMock = new();
    
    public WarningSentenceUnitTests()
    {
        _warningSentenceService = new WarningSentenceService(_warningSentenceReadRepositoryMock.Object, _warningSentenceRepositoryMock.Object);
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
        var exception = await Assert.ThrowsAsync<WarningSentencesNotFoundException>(() => _warningSentenceService.GetAllWarningSentencesAsync());

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
        var exception = await Assert.ThrowsAsync<WarningSentenceNotFoundException>(() => _warningSentenceService.GetWarningSentenceByIdAsync(fakeId));

        //Assert
        Assert.NotNull(exception);
    }
}