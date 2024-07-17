using System.Diagnostics;
using Json.Schema;
using Json.Schema.Generation;

internal class AnyOfTypeIntent: ISchemaKeywordIntent, IContextContainer
{
    private IEnumerable<SchemaGenerationContextBase> contexts;

    public AnyOfTypeIntent(IEnumerable<SchemaGenerationContextBase> contexts) 
    {
        this.contexts = contexts.Select((c) => {
            c.ReferenceCount++;
            return c;
        });
    }

    public void Apply(JsonSchemaBuilder builder)
	{
		builder.AnyOf(this.contexts.Select((c) => {

            return c.Apply();
        }).ToArray());
	}

    public void Replace(int hashCode, SchemaGenerationContextBase newContext)
    {
        this.contexts = this.contexts.Select((old) => (old.Hash == hashCode ? newContext : old));
    }
}