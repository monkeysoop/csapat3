using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MekkdonaldsModel.Persistence
{
    internal class EventConverter : JsonConverter<(int, int, string)>
    {
        public override (int, int, string) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            return (int.Parse(values[0]), int.Parse(values[1]), values[2]);
        }

        public override void Write(Utf8JsonWriter writer, (int, int, string) value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"[{value.Item1},{value.Item1},{value.Item3}");

        }
    }
}
