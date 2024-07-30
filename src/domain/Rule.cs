using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Rule
{
    [Required]
    public string Name { get; }

    [Default(100)]
    public int IntervalMilliseconds { get; }

    [Default(false)]
    public bool IsDebug { get; }

    [Required]
    public IBoolFunction Condition { get; }

    [Required]
    public ImmutableList<AbstractAction> Actions { get; }

    [JsonConstructor]
    public Rule(
        string name, 
        int intervalMilliseconds,
        bool isDebug,
        IBoolFunction condition, 
        ImmutableList<AbstractAction> actions
    ) {
        this.Name = name;
        this.IntervalMilliseconds = intervalMilliseconds;
        this.IsDebug = isDebug;
        this.Condition = condition;
        this.Actions = actions;
    }


}


