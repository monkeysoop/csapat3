using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence;

internal class LogFile
{
    internal static LogFile Load()
    {
        throw new NotImplementedException();
    }

    internal void Save()
    {
        throw new NotImplementedException();
    }

    [JsonConstructor]
    public Config() { }
}
