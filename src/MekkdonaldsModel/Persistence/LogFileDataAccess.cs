using System.Runtime.InteropServices;
using System.Text.Json;

namespace Mekkdonalds.Persistence;

public class LogFileDataAccess : ILogFileDataAccess
{
    public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, WriteIndented = true };

    public async Task<LogFile> LoadAsync(string path) => JsonSerializer.Deserialize<LogFile>(await File.ReadAllTextAsync(path), SerializerOptions) ?? throw new LogFileDataException();

    public async Task SaveAsync(string path, LogFile logFile) => await File.WriteAllTextAsync(path, JsonSerializer.Serialize(logFile, SerializerOptions));
}
