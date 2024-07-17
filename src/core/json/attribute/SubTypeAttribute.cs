
using System.Collections.Immutable;
using Json.More;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace NotifiableTools;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited=false)]
public class SubTypeAttribute(params Type[] subtypes) : Attribute, IAttributeHandler
{
    public Type[] Subtypes { get; } = subtypes;

    public void AddConstraints(SchemaGenerationContextBase context, Attribute attribute)
    {
        //var subContexts = this.Subtypes.Select((t) => SchemaGenerationContextCache.Get(t));
    }
}