using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence.Converters;

public class YesNoConverter : JsonConverter<bool>
{
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

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "Yes" : "No");


    }
}
