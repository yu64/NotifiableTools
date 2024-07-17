
using System.Collections.Immutable;
using Json.More;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace NotifiableTools;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited=false)]
public class UnionAttribute(params Type[] subtypes) : Attribute, IAttributeHandler
{
    public Type[] Subtypes { get; } = subtypes;

    public void AddConstraints(SchemaGenerationContextBase context, Attribute attribute)
    {
        if(context.Type != typeof(object))
        {
            throw new Exception("Required Union is object");
        }

        var subContexts = this.Subtypes.Select((t) => SchemaGenerationContextCache.Get(t));

        var anyOf = new AnyOfTypeIntent(subContexts);
        context.Intents.Add(anyOf);
    }
}