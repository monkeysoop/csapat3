namespace Mekkdonalds.Exception;

public class PathException : System.Exception
{
    public PathException()
    {
    }

    public PathException(string? message) : base(message)
    {
    }

    public PathException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
