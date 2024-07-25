using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Rule
{
    [Required]
    public string Name { get; }

    [Required]
    public IBoolFunction Condition { get; }

    [Required]
    public ImmutableList<AbstractAction> Actions { get; }

    [JsonConstructor]
    public Rule(string name, IBoolFunction condition, ImmutableList<AbstractAction> actions)
    {
        this.Name = name;
        this.Condition = condition;
        this.Actions = actions;
    }


}


