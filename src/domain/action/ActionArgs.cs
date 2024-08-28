
namespace NotifiableTools;


public readonly record struct ActionArgs(
    
    ActionDefinition Action,
    Rule Rule,
    INotion Notion,
    Timing Timing,
    IDictionary<string, object?> Custom

);

public enum Timing
{
    Start,
    Stop
}