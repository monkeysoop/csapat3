namespace Mekkdonalds.Exception;

internal class BoardDataException : System.Exception
{
    internal BoardDataException() { }
    internal BoardDataException(string message) : base(message) { }

    public BoardDataException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
