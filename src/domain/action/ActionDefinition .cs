

using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct ActionDefinition (

    [property: Description("SmartFormat")] string CommandTemplate

)
{
    public static readonly ActionDefinition NULL = new ActionDefinition("");

};