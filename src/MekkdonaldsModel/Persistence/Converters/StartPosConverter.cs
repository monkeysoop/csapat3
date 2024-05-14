using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

public class StartPosConverter : JsonConverter<List<(Point, Direction)>>
{
    /// <summary>
    /// reads the start positions from the json
    /// </summary>
    /// <param name="reader">reader object</param> 
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options">options for the serializer</param> 
    /// <returns>the start positions as a list of tuples</returns> 
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    public override List<(Point, Direction)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        List<(Point, Direction)> list = [];

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
    /// <summary>
    /// writes the start positions to the json
    /// </summary>
    /// <param name="writer">writer object</param> 
    /// <param name="value">start positions as a list of tuples to be written to the json</param> 
    /// <param name="options"> options for the serializer</param>
    public override void Write(Utf8JsonWriter writer, List<(Point, Direction)> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var (p, d) in value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(p.Y - 1);
            writer.WriteNumberValue(p.X - 1);
            writer.WriteStringValue(d.ToString()[0..1]);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
    /// <summary>
    /// reads a single start position value from the json
    /// </summary>
    /// <param name="reader">reader object</param> 
    /// <returns> a single start position as a tuple</returns>
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    private static (Point, Direction) ReadValue(ref Utf8JsonReader reader)
    {
        Point p = new();
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
                    p.Y = reader.GetInt32() + 1;
                }
                else
                {
                    p.X = reader.GetInt32() + 1;
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                d = DirectionMethods.Parse(reader.GetString() ?? throw new JsonException());
            }
        }

        throw new JsonException();
    }
}
