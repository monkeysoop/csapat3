namespace Mekkdonalds.Exception;

internal class LogFileDataException : System.Exception
{
    public LogFileDataException()
    {
    }

    public LogFileDataException(string? message) : base(message)
    {
    }

    public LogFileDataException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
