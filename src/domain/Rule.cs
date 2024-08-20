using System.Collections.Immutable;
using System.Text.Json.Serialization;
using System.Windows.Forms.VisualStyles;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Rule
{
    public string? Name { get; } = null;

    [Default(100)]
    [Minimum(-1)]
    public int IntervalMilliseconds { get; } = 100;

    [Default(true)]
    public bool Enable { get; } = true;

    [Required]
    public IBoolFunction Condition { get; }

    [Required]
    public ImmutableList<INotion> Notions { get; }

    [JsonConstructor]
    public Rule(
        string? name, 
        int intervalMilliseconds,
        bool enable,
        IBoolFunction condition, 
        ImmutableList<INotion> notions
    ) {
        this.Name = name;
        this.IntervalMilliseconds = intervalMilliseconds;
        this.Enable = enable;
        this.Condition = condition;
        this.Notions = notions;
    }


}


