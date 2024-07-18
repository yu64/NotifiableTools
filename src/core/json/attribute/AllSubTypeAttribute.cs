
using System.Collections.Immutable;
using Json.More;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace NotifiableTools;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited=true)]
public class AllSubTypeAttribute : Attribute
{

    public AllSubTypeAttribute()
    {
        //なし
    }

    public Type[] findAllSubType(Type superType)
    {
        var types = System.Reflection.Assembly.GetAssembly(typeof(AllSubTypeAttribute))
            ?.GetTypes()
            .Where(sub => sub.IsSubclassOf(superType) || sub.GetInterfaces().Contains(superType))
            .Where(sub => !sub.IsAbstract)
            .ToArray() ?? []
            ;

        return types;
    }
}