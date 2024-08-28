


using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct  Pipe (

    [property: Required] ActionDefinition StartAction,
    [property: Required] ActionDefinition StopAction

): INotion
{
    
}