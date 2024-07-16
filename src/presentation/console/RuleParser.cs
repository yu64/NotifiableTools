
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.More;
using Json.Schema;
using Json.Schema.Generation;

namespace NotifiableTools;

public class RuleParser
{

    public RuleParser()
    {

    }

    public string GenerateJsonSchema()
    {
        var builder = new JsonSchemaBuilder();
        var config = new SchemaGeneratorConfiguration()
        {
            Optimize = true,
            Refiners = { new SubTypeRefiner() }
        };

        var schema = builder.FromType<RuleSet>(config).Build();

        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(schema, options);
    }

    public string GenerateYamlSample(string sampleToSchemaPath)
    {
        return $"# yaml-language-server: $schema={sampleToSchemaPath}";
    }
}