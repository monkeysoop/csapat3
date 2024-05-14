using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

internal class ErrorConverter : JsonConverter<List<(int, int, int, string)>>
{
    /// <summary>
    /// custom json converter for Error list
    /// </summary>
    /// <param name="reader">name of the reader object</param> 
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options"> options for the serializer</param>
    /// <returns>list of tuples (The type of Errors)</returns> 
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    public override List<(int, int, int, string)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        List<(int, int, int, string)> list = [];

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
    /// writes the error list to the json
    /// </summary>
    /// <param name="writer">writer object</param> 
    /// <param name="value">error list as a list of tuples to be written to the json</param> 
    /// <param name="options"> options for the serializer</param>
    public override void Write(Utf8JsonWriter writer, List<(int, int, int, string)> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var (fst, snd, th, str) in value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(fst);
            writer.WriteNumberValue(snd);
            writer.WriteNumberValue(th);
            writer.WriteStringValue(str);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
    /// <summary>
    /// reads a single error value from the json
    /// </summary>
    /// <param name="reader">reader object</param> 
    /// <returns>a single error value as a tuple</returns> 
    /// <exception cref="JsonException"> throws exception if the json is not in the correct format</exception>
    private static (int, int, int, string) ReadValue(ref Utf8JsonReader reader)
    {
        int fst = 0, snd = 0, th = 0, i = 0, j = 0;
        string str = "";

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return (fst, snd, th, str);
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (i++ == 0)
                {
                    fst = reader.GetInt32();
                }
                else
                {
                    if (j++ == 0)
                    {
                        snd = reader.GetInt32();
                    }
                    else
                    {
                        th = reader.GetInt32();
                    }
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
