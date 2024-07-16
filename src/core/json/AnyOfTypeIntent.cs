using Json.Schema;
using Json.Schema.Generation;

internal class AnyOfTypeIntent(IEnumerable<SchemaGenerationContextBase> contexts) : ISchemaKeywordIntent, IContextContainer
{
    private IEnumerable<SchemaGenerationContextBase> contexts = contexts;

    public void Apply(JsonSchemaBuilder builder)
	{
		builder.AnyOf(this.contexts.Select((c) => {

            var subBuilder = new JsonSchemaBuilder();
            c.Apply(subBuilder);
            return (JsonSchema)subBuilder;
        }));
	}

    public void Replace(int hashCode, SchemaGenerationContextBase newContext)
    {
        this.contexts = this.contexts.Select((old) => (old.Hash == hashCode ? newContext : old));
    }
}