

using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct ActionDefinition (

    [property: Description("SmartFormat")] string CommandTemplate,
    
    [property: Default(false)]
    [property: Description("標準出力可能か")]
    bool CanStdOut = false

)
{
    public static readonly ActionDefinition NULL = new ActionDefinition("");

};