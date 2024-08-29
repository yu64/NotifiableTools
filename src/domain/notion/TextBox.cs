using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct TextBox (

    
    [property: Required] string DefaultText,
    [property: Required] IPosition Position,
    [property: Required] ActionDefinition Action
    
) : INotion
{
    
}