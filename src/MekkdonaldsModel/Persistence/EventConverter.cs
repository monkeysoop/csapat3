using System.Text.Json;
using System.Text.Json.Serialization;

namespace MekkdonaldsModel.Persistence
{
    internal class EventConverter : JsonConverter<List<List<(int, int, string)>>>
    {
        public override List<List<(int, int, string)>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("\t", "");
            value = value.Replace(" ", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            List<List<(int, int, string)>> lista = new();
            return lista;
        }

        public override void Write(Utf8JsonWriter writer, List<List<(int, int, string)>> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"[{value}");

        }
    }
}
