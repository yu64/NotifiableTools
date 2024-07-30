using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Rule
{
    public string? Name { get; }

    [Default(100)]
    public int IntervalMilliseconds { get; }

    [Obsolete("Ruleの非同期処理において、ログ出力は都合が悪く本格的に実装されていないため、非推奨")]
    [Default(false)]
    public bool EnableLog { get; }

    [Required]
    public IBoolFunction Condition { get; }

    [Required]
    public ImmutableList<AbstractAction> Actions { get; }

    [JsonConstructor]
    public Rule(
        string? name, 
        int intervalMilliseconds,
        bool enableLog,
        IBoolFunction condition, 
        ImmutableList<AbstractAction> actions
    ) {
        this.Name = name;
        this.IntervalMilliseconds = intervalMilliseconds;
        this.EnableLog = enableLog;
        this.Condition = condition;
        this.Actions = actions;
    }


}


