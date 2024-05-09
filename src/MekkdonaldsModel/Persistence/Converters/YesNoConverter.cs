using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

public class YesNoConverter : JsonConverter<bool>
{
    /// <summary>
    /// Custom json converter for converting Yes to true and No to false
    /// </summary>
    /// <param name="reader">reader object</param> 
    /// <param name="typeToConvert">type of the object to convert</param> 
    /// <param name="options"> options for the serializer</param>
    /// <returns>the boolean value </returns> 
    /// <exception cref="JsonException">throws exception if the json is not in the correct format</exception> 
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? yesorno = reader.GetString();

        return yesorno switch
        {
            "Yes" => true,
            "No" => false,
            _ => throw new JsonException()
        };
    }
    /// <summary>
    /// writes the boolean value to the json as Yes or No
    /// </summary>
    /// <param name="writer">writer object</param> 
    /// <param name="value">boolean value to be written to the json</param> 
    /// <param name="options">options for the serializer</param> 
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "Yes" : "No");


    }
}
