using System.Text.Json;
using System.Text.Json.Serialization;

namespace MekkdonaldsModel.Persistence
{
    internal class ErrorConverter : JsonConverter<(int, int, int, string)>
    {
        public override (int, int, int, string) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            return (int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), values[3]);
        }

        public override void Write(Utf8JsonWriter writer, (int, int, int, string) value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"[{value.Item1},{value.Item1},{value.Item3},{value.Item4}");

        }
    }
}
