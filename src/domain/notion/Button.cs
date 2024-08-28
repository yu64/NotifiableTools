


using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct Button(

    [property: Required] string Title,
    [property: Required] IPosition Position,
    [property: Required] ActionDefinition Action

) : INotion
{
    


}