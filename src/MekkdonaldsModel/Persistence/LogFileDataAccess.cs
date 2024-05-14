using System.Text.Json;

namespace Mekkdonalds.Persistence;
/// <summary>
/// Data access for the log files
/// </summary>
public class LogFileDataAccess : ILogFileDataAccess
{
    /// <summary>
    /// Naming policy for the JSON serializer
    /// </summary>
    public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, WriteIndented = true };
    /// <summary>
    /// Loads a log file from the given path
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <returns>A task that represents the Loading operation. The task result contains the log file</returns> 
    /// <exception cref="LogFileDataException">Thrown when the data is invalid</exception> 
    public async Task<LogFile> LoadAsync(string path) => JsonSerializer.Deserialize<LogFile>(await File.ReadAllTextAsync(path), SerializerOptions) ?? throw new LogFileDataException();
    /// <summary>
    /// Saves a log file to the given path
    /// </summary>
    /// <param name="path"> Path to the file</param>
    /// <param name="logFile"> Log file to save</param>
    /// <returns>A task that represents the save operation</returns> 
    public async Task SaveAsync(string path, LogFile logFile) => await File.WriteAllTextAsync(path, JsonSerializer.Serialize(logFile, SerializerOptions));
}
