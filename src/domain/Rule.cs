


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NotifiableTools;


public record struct Rule
{
    [Required]
    public readonly string Name { get; }

    [Required]
    public readonly ICondition Condition { get; }

    public Rule(string name, ICondition condition)
    {
        this.Name = name;
        this.Condition = condition;
    }


}