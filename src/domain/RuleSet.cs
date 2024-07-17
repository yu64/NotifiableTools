


using System.Collections.Immutable;
using Json.Schema.Generation;

namespace NotifiableTools;


public record struct RuleSet
{
    [Required]
    public readonly ImmutableList<Rule> Rules { get; }

    public RuleSet(ImmutableList<Rule> Rules)
    {
        this.Rules = Rules;
    }

    public RuleSet Merge(RuleSet left)
    {
        var rules = this.Rules.Concat(left.Rules);
        return new RuleSet(ImmutableList.CreateRange(rules));
    }
}