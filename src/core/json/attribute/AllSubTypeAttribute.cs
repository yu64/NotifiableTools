
using System.Collections.Immutable;
using Json.More;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace NotifiableTools;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited=false)]
public class AllSubTypeAttribute : Attribute, IAttributeHandler
{

    public AllSubTypeAttribute()
    {
        //なし
    }

    public Type[] findAllSubType(Type superType)
    {
        var types = System.Reflection.Assembly.GetAssembly(typeof(SubTypeAttribute))
            ?.GetTypes()
            .Where(sub => sub.IsSubclassOf(superType) || sub.GetInterfaces().Contains(superType))
            .Where(sub => !sub.IsAbstract)
            .ToArray() ?? []
            ;

        return types;
    }

    public void AddConstraints(SchemaGenerationContextBase context, Attribute attribute)
    {
        // var subtypes = this.findAllSubType(context.Type);
        // var subContexts = subtypes.Select((t) => SchemaGenerationContextCache.Get(t));
    }
}