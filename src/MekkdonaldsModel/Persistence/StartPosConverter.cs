using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MekkdonaldsModel.Persistence
{
    public class StartPosConverter : JsonConverter<(Point, Direction)>
    {
        public override (Point, Direction) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString() ?? throw new NullReferenceException();
            value = value.Replace("\n", "");
            value = value.Replace("[", "");
            value = value.Replace("]", "");
            var values = value.Split(",");
            return (new Point(int.Parse(values[0]), int.Parse(values[1])), DirectionMethods.StringToDirection(values[2]));
        }

        public override void Write(Utf8JsonWriter writer, (Point, Direction) value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"[{value.Item1.X},{value.Item1.Y},{DirectionMethods.DirectionToString(value.Item2)}");
        }
    }
}
