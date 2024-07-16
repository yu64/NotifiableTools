


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NotifiableTools;


public readonly record struct Rule
{
    [Required]
    public readonly string Name { get; }

    [Required]
    public readonly Condition Condition { get; }

    public Rule(string name, Condition condition)
    {
        this.Name = name;
        this.Condition = condition;
    }


}