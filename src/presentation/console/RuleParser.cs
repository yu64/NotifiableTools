
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.More;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Serialization;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace NotifiableTools;

public class RuleParser
{

    private readonly JsonSerializerOptions serializerOptions;

    public RuleParser()
    {
        Type? GetTypeFromSimpleName(String typeName)
        {
            return Type.GetType($"{this.GetType().Namespace}.{typeName}");
        }
        
        this.serializerOptions = new JsonSerializerOptions() 
        { 
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
            Converters = {
                new SubTypeConverter<object>(GetTypeFromSimpleName),
                new TextToBoolConverter()
            }
        };
    }

    public JsonSchema GenerateJsonSchemaObj()
    {
        var builder = new JsonSchemaBuilder();
        var config = new SchemaGeneratorConfiguration()
        {
            Optimize = true,
            Refiners = { new SubTypeRefiner() }
        };

        var schema = builder.FromType<RuleSet>(config).Build();
        return schema;
    }

    public string GenerateJsonSchema()
    {
        return JsonSerializer.Serialize(this.GenerateJsonSchemaObj(), this.serializerOptions);
    }

    public string GenerateYamlSample(string sampleToSchemaPath)
    {
        return $"# yaml-language-server: $schema={sampleToSchemaPath}";
    }


    public RuleSet ParseFromFile(string path)
    {
        var text = File.ReadAllText(path);
        var ext = Path.GetExtension(path);

        return ext switch
        {
            ".yaml" => this.ParseFromYaml(text),
            ".json" => this.ParseFromJson(text),
            _ => throw new Exception("Unsupport rule format")
        };
    }


    public RuleSet ParseFromYaml(string yaml)
    {
        var deserializer = new Deserializer();
        var yamlObject = deserializer.Deserialize(yaml);
        var serializer = new SerializerBuilder()
            .JsonCompatible()
            .Build();

        var json = serializer.Serialize(yamlObject).Trim();
        return this.ParseFromJson(json);
    }

    public RuleSet ParseFromJson(string json)
    {
        return JsonSerializer.Deserialize<RuleSet>(json, this.serializerOptions);
    }






}