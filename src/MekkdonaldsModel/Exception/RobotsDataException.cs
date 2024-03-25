namespace Mekkdonalds.Exception;

internal class RobotsDataException : System.Exception
{
    internal RobotsDataException() { }
    internal RobotsDataException(string message) : base(message) { }

    public RobotsDataException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
