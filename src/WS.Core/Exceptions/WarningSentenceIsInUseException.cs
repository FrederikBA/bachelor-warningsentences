namespace WS.Core.Exceptions;

public class WarningSentenceIsInUseException : Exception
{
    public WarningSentenceIsInUseException(int id) : base($"Warning sentence with id {id} is in use.")
    {
    }
}