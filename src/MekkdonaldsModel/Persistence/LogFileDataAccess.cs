using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mekkdonalds.Persistence
{
    public class LogFileDataAccess : ILogFileDataAccess
    {
        public static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<LogFile> Load(string path) => JsonSerializer.Deserialize<LogFile>(await File.ReadAllTextAsync(path), SerializerOptions) ?? throw new LogFileDataException();
    }
}
