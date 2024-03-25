using System.Text.Json;
using System.Text.Json.Serialization;

namespace MekkdonaldsModel.Persistence
{
    public class StartPosConverter : JsonConverter<List<(Point, Direction)>>
    {
        public override List<(Point, Direction)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("\t", "");
            value = value.Replace(" ", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            var listagenyo = new List<(Point, Direction)>();
            for (int i = 0; i < values.Length; i += 3)
            {
                var tuplegenyo = (new Point(int.Parse(values[i]), int.Parse(values[i + 1])), DirectionMethods.StringToDirection(values[i + 2]));
                listagenyo.Add(tuplegenyo);
            }
            return listagenyo;
        }

        public override void Write(Utf8JsonWriter writer, List<(Point, Direction)> value, JsonSerializerOptions options)
        {
            for (int i = 0; i < value.Count; i++)
            {
                writer.WriteStringValue($"[{value[i].Item1.X},{value[i].Item1.Y},{DirectionMethods.DirectionToString(value[i].Item2)}");

            }
        }
    }
}
