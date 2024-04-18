namespace WS.Core.Exceptions;

public class WarningSentenceNotFoundException : Exception
{
    public WarningSentenceNotFoundException(int warningSentenceId) : base($"No warning sentence with id {warningSentenceId} was found.")
    {
    }
}