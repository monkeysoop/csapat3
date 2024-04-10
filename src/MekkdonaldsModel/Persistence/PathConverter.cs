using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MekkdonaldsModel.Persistence
{
    public class PathConverter : JsonConverter<List<List<string>>>
    {
        public override List<List<string>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            var list = new List<List<string>>();
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
                    var arr = (str.Trim().Split(",")).ToList();
                    list.Add(arr);
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, List<List<string>> value, JsonSerializerOptions options)
        {
            string str;
            writer.WriteStartArray();
            foreach (var item in value)
            {
                str = "";
                int i = 0;
                foreach (var s in item)
                {
                    if (i != item.Count)
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
}
