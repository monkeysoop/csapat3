using System.Text.Json;
using System.Text.Json.Serialization;

namespace MekkdonaldsModel.Persistence
{
    public class StartPosConverter : JsonConverter<List<(Point, Direction)>>
    {
        public override List<(Point, Direction)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            var list = new List<(Point, Direction)>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return list;
                }

                list.Add(ReadValue(ref reader));
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, List<(Point, Direction)> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var (p, d) in value)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(p.X);
                writer.WriteNumberValue(p.Y);
                writer.WriteStringValue(d.ToString()[0..1]);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }

        private static (Point, Direction) ReadValue(ref Utf8JsonReader reader)
        {
            var p = new Point();
            var d = Direction.North;
            int i = 0;

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return (p, d);
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    if (i++ == 0)
                    {
                        p.X = reader.GetInt32();
                    }
                    else
                    {
                        p.Y = reader.GetInt32();
                    }
                }                
                else if (reader.TokenType == JsonTokenType.String)
                {
                    d = DirectionMethods.StringToDirection(reader.GetString() ?? throw new JsonException());
                }
            }

            throw new JsonException();
        }
    }
}
