using System.Text.Json;
using System.Text.Json.Serialization;
using Json.More;
using NotifiableTools;

namespace Json.Schema.Generation;


public class TextToBoolConverter : JsonConverter<bool>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(bool);
    }


    public override bool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.True)
        {
            return true;
        }

        if (reader.TokenType == JsonTokenType.False)
        {
            return false;
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Not bool or string => Result is not bool");
        }

        //JSONを読み取る
        Utf8JsonReader copiedReader = reader;
        var text = copiedReader.GetString();

        if( text == null)
        {
            throw new JsonException();
        }

        if (String.Equals(text.ToLower().Trim(), "true"))
        {
            return true;
        }

        if (String.Equals(text.ToLower().Trim(), "false"))
        {
            return false;
        }

        throw new JsonException($"The string \"{text}\" is not a bool");
    }


    public override void Write(
        Utf8JsonWriter writer,
        bool value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(bool), options);
    }
}






