using System.Windows;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;
using NotifiableTools;

internal class SubTypeRefiner : ISchemaRefiner
{
    public bool ShouldRun(SchemaGenerationContextBase context)
    {
        // we only want to run this if the generated schema has a `type` keyword
        return context.Intents.OfType<TypeIntent>().Any();
    }

    public void Run(SchemaGenerationContextBase context)
    {
        var attributes = context.Type.GetCustomAttributes(false);
        var subTypes = attributes
            .OfType<SubTypeAttribute>()
            .SelectMany((a) => a.Subtypes)
            .Concat(
                attributes.OfType<AllSubTypeAttribute>()
                    .SelectMany((a) => a.findAllSubType(context.Type))
            )
            .Distinct()
            .ToList();

        if(subTypes.Count == 0)
        {
            return;
        }

        

        var subContexts = subTypes.Select((t) => SchemaGenerationContextCache.Get(t)).ToArray();

        var anyOf = new AnyOfTypeIntent(subContexts);
        context.Intents.Add(anyOf);
    }
}