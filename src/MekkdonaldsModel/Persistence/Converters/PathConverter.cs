using System.Text.Json;
using System.Text.Json.Serialization;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Persistence.Converters;

public class PathConverter : JsonConverter<List<List<Action>>>
{
    /// <summary>
    /// custom json converter for Path list
    /// </summary>
    /// <param name="reader"> reader object</param>
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options"> options for the serializer</param>
    /// <returns> list of lists of Actions</returns>
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    public override List<List<Action>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }
        List<List<Action>> list = [];
        string str;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return list;
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                str = reader.GetString() ?? throw new JsonException();
                List<Action> arr = str.Trim().Split(",").Select(x => (Action)Enum.Parse(typeof(Action), x)).ToList();
                list.Add(arr);
            }
        }
        throw new JsonException();
    }
    /// <summary>
    /// writes the path list to the json
    /// </summary>
    /// <param name="writer">writer object</param> 
    /// <param name="value">path list as a list of lists of Actions to be written to the json</param> 
    /// <param name="options"> options for the serializer</param>
    public override void Write(Utf8JsonWriter writer, List<List<Action>> value, JsonSerializerOptions options)
    {
        string str;
        writer.WriteStartArray();
        foreach (var item in value)
        {
            str = "";
            int i = 0;
            foreach (var s in item)
            {
                if (i != item.Count - 1)
                {
                    str += $"{s},";

                }
                else
                {
                    str += s;
                }
                i++;
            }
            writer.WriteStringValue(str);
        }
        writer.WriteEndArray();
    }
}
