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
            value = value.Replace("[[", "lb");
            value = value.Replace("]]", "le");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split("lb");
            List<string> stringl = new();
            foreach (var item in values)
            {
                if (item != "") { stringl.Add(item); }
            }
            List<List<(int, int, string)>> lista = new();
            List<(int, int, string)> kislista = new();
            foreach (var item in stringl)
            {
                values = item.Split(',');
                kislista.Clear();
                for (int i = 0; i < values.Length; i += 3)
                {
                    var tuple = (int.Parse(values[i]), int.Parse(values[i + 1]), values[i + 1]);
                    kislista.Add(tuple);
                }
                lista.Add(kislista);

            }
            return lista;
        }

        public override void Write(Utf8JsonWriter writer, List<List<(int, int, string)>> value, JsonSerializerOptions options)
        {
            foreach (var item in value)
            {
                writer.WriteStringValue($"[");
                foreach (var item2 in item)
                {
                    writer.WriteStringValue($"[{item2.Item1},{item2.Item2},{item2.Item3}]");
                }
                writer.WriteStringValue($"]");
            }


        }
    }
}
