using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

internal class EventConverter : JsonConverter<List<List<(int, int, string)>>>
{
    public override List<List<(int, int, string)>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        var list = new List<List<(int, int, string)>>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return list;
            }

            list.Add(ReadList(ref reader));
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, List<List<(int, int, string)>> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStartArray();
            foreach (var (fst, snd, th) in item)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(fst);
                writer.WriteNumberValue(snd);
                writer.WriteStringValue(th);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }

    private static List<(int, int, string)> ReadList(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        var list = new List<(int, int, string)>();

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

    private static (int, int, string) ReadValue(ref Utf8JsonReader reader)
    {
        int fst = 0, snd = 0, i = 0;
        string str = "";

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return (fst, snd, str);
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (i++ == 0)
                {
                    fst = reader.GetInt32();
                }
                else
                {
                    snd = reader.GetInt32();
                }

            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                str = reader.GetString() ?? throw new JsonException();
            }
        }

        throw new JsonException();
    }
}
