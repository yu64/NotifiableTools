
using System.Text.Json;
using System.Text.Json.Serialization;
using NJsonSchema.Generation;

namespace NotifiableTools;

public class RuleParser
{

    public RuleParser()
    {

    }

    public string GenerateJsonSchema()
    {
        var settings = new SystemTextJsonSchemaGeneratorSettings
        {
            SerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters = {
                    new JsonStringEnumConverter()
                }
            }
        };
        
        var generator = new JsonSchemaGenerator(settings);
        var schema = generator.Generate(typeof(RuleSet));
        return schema.ToJson();
    }

    public string GenerateYamlSample(string sampleToSchemaPath)
    {
        return $"# yaml-language-server: $schema={sampleToSchemaPath}";
    }
}