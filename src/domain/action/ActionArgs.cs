
namespace NotifiableTools;


public readonly record struct ActionArgs(
    
    ActionDefinition Action,
    Rule Rule,
    INotion Notion,
    Timing Timing

);

public enum Timing
{
    Start,
    Stop
}