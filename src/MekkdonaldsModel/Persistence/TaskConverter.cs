using System.Text.Json;
using System.Text.Json.Serialization;

namespace MekkdonaldsModel.Persistence
{
    internal class TaskConverter : JsonConverter<List<(int, int, int)>>
    {
        public override List<(int, int, int)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("\t", "");
            value = value.Replace(" ", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            var lista = new List<(int, int, int)>();
            for (int i = 0; i < values.Length; i += 4)
            {
                var tuple = (int.Parse(values[i]), int.Parse(values[i + 1]), int.Parse(values[i + 2]));
                lista.Add(tuple);
            }
            return lista;
        }

        public override void Write(Utf8JsonWriter writer, List<(int, int, int)> value, JsonSerializerOptions options)
        {
            for (int i = 0; i < value.Count; i++)
            {
                writer.WriteStringValue($"[{value[i].Item1},{value[i].Item2},{value[i].Item3}]");

            }
        }
    }
}
