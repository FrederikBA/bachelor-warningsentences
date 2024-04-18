namespace WS.Core.Exceptions;

public class WarningSentencesNotFoundException : Exception
{
    public WarningSentencesNotFoundException() : base("No warning sentences were found.")
    {
    }
}