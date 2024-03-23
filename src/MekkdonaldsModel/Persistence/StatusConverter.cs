using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence;

public class StrategyConverter : JsonConverter<Strategy>
{
    public override Strategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var strategy = reader.GetString();

        return strategy switch
        {
            "roundrobin" => Strategy.RoundRobin,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, Strategy value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower());
    }
}
