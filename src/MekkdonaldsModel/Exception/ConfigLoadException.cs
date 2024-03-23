namespace Mekkdonalds.Exception;

internal class ConfigLoadException : System.Exception
{
    internal ConfigLoadException() { }
    internal ConfigLoadException(string message) : base(message) { }
    public ConfigLoadException(string? message, System.Exception? innerException) : base(message, innerException) { }
}
