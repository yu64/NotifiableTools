
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

    public RuleParser()
    {

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
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(this.GenerateJsonSchemaObj(), options);
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
        var options = new JsonSerializerOptions() 
        { 
            WriteIndented = true,
            Converters = {
                new SubTypeConverter<IBoolFunction>()
            }
        };
        return JsonSerializer.Deserialize<RuleSet>(json);
    }






}