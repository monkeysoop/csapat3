using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MekkdonaldsModel.Persistence
{
    internal class TaskConverter : JsonConverter<(int, int, int)>
    {
        public override (int, int, int) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            return (int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
        }

        public override void Write(Utf8JsonWriter writer, (int, int, int) value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"[{value.Item1},{value.Item1},{value.Item3}");
        }
    }
}
