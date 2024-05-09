using System.Text.Json;

namespace Mekkdonalds.Persistence;

public class ConfigDataAccess : IConfigDataAccess
{
    /// <summary>
    /// naming policy for the json serializer
    /// </summary>
    public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    /// <summary>
    /// loads the config from the given path
    /// </summary>
    /// <param name="path">őath to the file</param> 
    /// <returns>task that represents the loading operation. The task result contains the config</returns> 
    /// <exception cref="ConfigDataException">thrown when the data is invalid</exception> 
    public async Task<Config> LoadAsync(string path) => JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync(path), SerializerOptions) ?? throw new ConfigDataException();
}
