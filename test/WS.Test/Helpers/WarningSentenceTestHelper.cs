using WS.Core.Entities.WSAggregate;
using WS.Core.Models.Dtos;

namespace WS.Test.Helpers;

public static class WarningSentenceTestHelper
{
    public static List<WarningSentence> GetTestWarningSentences()
    {
        return new List<WarningSentence>
        {
            new WarningSentence
            {
                Id = 1,
                Text = "Test Warning Sentence 1",
                WarningCategory = GetTestWarningCategory(),
                WarningSignalWord = GetTestWarningSignalWord(),
                WarningPictogram = GetTestWarningPictogram()
            },
            new WarningSentence
            {
                Id = 2,
                Text = "Test Warning Sentence 2",
            }
        };
    }
    
    private static WarningType GetTestWarningType()
    {
        return new WarningType
        {
            Id = 1,
            Type = "Test Warning Type"
        };
    }
    
    private static WarningCategory GetTestWarningCategory()
    {
        return new WarningCategory
        {
            Id = 1,
            Text = "Test Warning Category",
            WarningType = GetTestWarningType()
        };
    }
    
    private static WarningSignalWord GetTestWarningSignalWord()
    {
        return new WarningSignalWord
        {
            Id = 1,
            SignalWordText = "Test Warning Signal Word"
        };
    }
    
    private static WarningPictogram GetTestWarningPictogram()
    {
        return new WarningPictogram
        {
            Id = 1,
            Code = "GHS01-Test",
            Pictogram = "TestPictogram",
            Extension = "webp",
            Text = "Corrosives",
        };
    }

    public static WarningSentenceDto GetTestWarningSentenceDto()
    {
        return new WarningSentenceDto
        {
            Code = "TestCode",
            Text = "Test Warning Sentence",
            WarningTypeId = 1,
            WarningCategoryId = 1,
            WarningSignalWordId = 1,
            WarningPictogramId = 1
        };
    }
}