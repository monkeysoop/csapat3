namespace Mekkdonalds.Exception;

internal class ConfigDataException : System.Exception
{
    internal ConfigDataException() { }
    internal ConfigDataException(string message) : base(message) { }
    public ConfigDataException(string? message, System.Exception? innerException) : base(message, innerException) { }
}
