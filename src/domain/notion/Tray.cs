using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct  Tray (

    [property: Required] ActionDefinition Action
    
) : INotion
{
    
}