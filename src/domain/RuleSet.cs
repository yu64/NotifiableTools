


using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NotifiableTools;


public record struct RuleSet
{
    [Required]
    public readonly ImmutableList<Rule> Rules { get; }

    public RuleSet(ImmutableList<Rule> Rules)
    {
        this.Rules = Rules;
    }


}