using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

internal class TaskConverter : JsonConverter<List<(int, int, int)>>
{
    /// <summary>
    /// custom json converter for Tasks
    /// </summary>
    /// <param name="reader">reader object</param> 
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options">options for the serializer</param> 
    /// <returns>list of tuples (The type of Tasks)</returns> 
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    public override List<(int, int, int)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        List<(int, int, int)> list = [];

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
    /// writes the task list to the json
    /// </summary>
    /// <param name="writer"> writer object</param>
    /// <param name="value">task list as a list of tuples to be written to the json</param> 
    /// <param name="options">options for the serializer</param> 
    public override void Write(Utf8JsonWriter writer, List<(int, int, int)> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var (fst, snd, th) in value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(fst);
            writer.WriteNumberValue(snd);
            writer.WriteNumberValue(th);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
    /// <summary>
    /// reads a single task value from the json
    /// </summary>
    /// <param name="reader">reader object</param>
    /// <returns>a single task as a tuple</returns> 
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    private static (int, int, int) ReadValue(ref Utf8JsonReader reader)
    {
        int fst = 0, snd = 0, th = 0, i = 0, j = 0;

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return (fst, snd, th);
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
        }

        throw new JsonException();
    }
}
