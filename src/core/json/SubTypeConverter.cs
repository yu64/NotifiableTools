

using System.CommandLine.Parsing;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.More;
using NotifiableTools;

namespace Json.Schema.Generation;

public class SubTypeConverter<TSuper> : JsonConverter<TSuper>
{



    
    public override TSuper Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Utf8JsonReader copiedReader = reader;
        
        

        return (TSuper)JsonSerializer.Deserialize(ref reader, typeof(IsExistFunction), options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TSuper value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(TSuper), options);
    }
}