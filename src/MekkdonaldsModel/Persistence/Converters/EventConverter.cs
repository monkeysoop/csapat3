using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

internal class EventConverter : JsonConverter<List<List<(int, int, string)>>>
{
    /// <summary>
    /// custom json converter for List of Event lists
    /// </summary>
    /// <param name="reader"> reader object</param>
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options">options for the serializer</param> 
    /// <returns>list of lists of tuples (The type of Events)</returns> 
    /// <exception cref="JsonException"> throws exception if the json is not in the correct format</exception>
    public override List<List<(int, int, string)>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        List<List<(int, int, string)>> list = [];

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

    /// <summary>
    /// writes the event list to the json
    /// </summary>
    /// <param name="writer">writer object</param> 
    /// <param name="value">List of  Event lists to be written to the json</param> 
    /// <param name="options">options for the serializer</param> 
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
    /// <summary>
    /// reads a list of events from the json
    /// </summary>
    /// <param name="reader"> the reader object</param>
    /// <returns>list of tuples (a list of events)</returns>  
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    private static List<(int, int, string)> ReadList(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        List<(int, int, string)> list = [];

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
    /// reads a single event value from the json
    /// </summary>
    /// <param name="reader">the reader object</param> 
    /// <returns>tuple of event values (a single event)</returns> 
    /// <exception cref="JsonException"> throws exception if the json is not in the correct format</exception>
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
