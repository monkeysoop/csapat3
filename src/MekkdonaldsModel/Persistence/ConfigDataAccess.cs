using System.Text.Json;

namespace Mekkdonalds.Persistence;

internal class ConfigDataAccess : IConfigDataAccess
{
    public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<Config> Load(string path) => JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync(path), SerializerOptions) ?? throw new ConfigDataException();
}
