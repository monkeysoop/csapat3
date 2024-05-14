using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

public class StrategyConverter : JsonConverter<Strategy>
{
    /// <summary>
    /// loads the strategy from the json
    /// </summary>
    /// <param name="reader">reader object</param> 
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options"> options for the serializer</param>
    /// <returns>the strategy</returns> 
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    public override Strategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? strategy = reader.GetString();

        return strategy switch
        {
            "roundrobin" => Strategy.RoundRobin,
            _ => throw new JsonException()
        };
    }

    /// <summary>
    /// writes the strategy to the json
    /// </summary>
    /// <param name="writer">writer object</param>
    /// <param name="value"> value of the strategy to be written to the json </param> 
    /// <param name="options">options for the serializer</param> 
    public override void Write(Utf8JsonWriter writer, Strategy value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower());
    }
}
