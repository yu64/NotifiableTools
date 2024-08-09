


using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct RuleSet
{
    [Required]
    public ImmutableList<Rule> Rules { get; }

    [JsonConstructor]
    public RuleSet(ImmutableList<Rule> Rules)
    {
        this.Rules = Rules;
    }

    public RuleSet Merge(RuleSet left)
    {
        var rules = this.Rules.Concat(left.Rules);
        return new RuleSet(ImmutableList.CreateRange(rules));
    }

    public ImmutableList<Rule> GetEnableRules()
    {
        return this.Rules.Where((r) => r.Enable).ToImmutableList();
    }
}