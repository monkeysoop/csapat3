using System.Text.Json;
using System.Text.Json.Serialization;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Persistence.Converters;

public class PathConverter : JsonConverter<List<List<Action>>>
{
    public override List<List<Action>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }
        var list = new List<List<Action>>();
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
                var arr = str.Trim().Split(",").Select(x => (Action)Enum.Parse(typeof(Action), x)).ToList();
                list.Add(arr);
            }
        }
        throw new JsonException();
    }

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
