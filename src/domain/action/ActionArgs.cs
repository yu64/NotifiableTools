
namespace NotifiableTools;


public readonly record struct ActionArgs(
    
    ActionDefinition Action,
    Rule Rule,
    INotion Notion,
    Timing Timing,
    string InputText,
    IDictionary<string, object?> Custom

);

public enum Timing
{
    Start,
    Stop
}