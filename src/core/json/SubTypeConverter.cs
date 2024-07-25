using System.Text.Json;
using System.Text.Json.Serialization;
using Json.More;
using NotifiableTools;

namespace Json.Schema.Generation;


public class SubTypeConverter<TSuper> : JsonConverter<TSuper>
{
    private readonly Func<string, Type?> getType;
    private readonly string typePropertyName;

    public SubTypeConverter(Func<string, Type?> getType, string typePropertyName = "$type")
    {
        this.getType = getType;
        this.typePropertyName = typePropertyName;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        //出力クラスとして扱えない場合、除外
        if(!typeof(TSuper).IsAssignableFrom(typeToConvert))
        {
            return false;
        }


        //インスタンス化可能である場合、除外
        if(!typeToConvert.IsAbstract && !typeToConvert.IsInterface)
        {
            return false;
        }

        //サブタイプがあるとマークされていない場合、除外
        if(!typeToConvert.GetCustomAttributes(false).Any((t) => t is AllSubTypeAttribute))
        {
            return false;
        }

        return true;
    }


    public override TSuper Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        //JSONを読み取る
        Utf8JsonReader copiedReader = reader;
        using var doc = JsonDocument.ParseValue(ref copiedReader);

        //サブタイプ名を取得
        var subTypeName = doc.RootElement.GetProperty(this.typePropertyName).GetString();
        if(subTypeName == null)
        {
            throw new JsonException($"Not Found Property {this.typePropertyName}");
        }

        //サブタイプを取得
        Type subType = this.getType(subTypeName) ?? throw new JsonException($"Not Found SubType {subTypeName}");
        
        return (TSuper)JsonSerializer.Deserialize(ref reader, subType, options)!;
    }


    public override void Write(
        Utf8JsonWriter writer,
        TSuper value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(TSuper), options);
    }
}






