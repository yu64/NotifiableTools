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

        var subTypes = context.Type.GetCustomAttributes(false)
            .OfType<SubTypeAttribute>()
            .SelectMany((a) => a.Subtypes)
            .Distinct()
            .ToList();

        if(subTypes.Count == 0)
        {
            return;
        }

        System.Console.WriteLine(context.Type);
        System.Console.WriteLine(context);
        context.Intents.ToList().ForEach(Console.WriteLine);

        var subContexts = subTypes.Select((t) => SchemaGenerationContextCache.Get(t));

        var subIntents = subContexts.Select((c) => {

            var list = new List<ISchemaKeywordIntent>();
            return list.Concat(c.Intents.OfType<RequiredIntent>())
            .Concat(c.Intents.OfType<PropertiesIntent>());
        });

        var anyOf = new AnyOfTypeIntent(subContexts);
        context.Intents.Add(anyOf);
    }
}