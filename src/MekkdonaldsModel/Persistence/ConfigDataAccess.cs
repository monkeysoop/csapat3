using System.Text.Json;

namespace Mekkdonalds.Persistence;
/// <summary>
/// Data access for the config
/// </summary>
public class ConfigDataAccess : IConfigDataAccess
{
    /// <summary>
    /// Naming policy for the json serializer
    /// </summary>
    public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    /// <summary>
    /// Loads the config from the given path
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <returns>Task that represents the loading operation. The task result contains the config</returns> 
    /// <exception cref="ConfigDataException">Thrown when the data is invalid</exception> 
    public async Task<Config> LoadAsync(string path) => JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync(path), SerializerOptions) ?? throw new ConfigDataException();
}
