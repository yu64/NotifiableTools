using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Rule
{
    [Required]
    public string Name { get; }

    [Required]
    public IAnyFunction Condition { get; }

    [JsonConstructor]
    public Rule(string name, IAnyFunction condition)
    {
        this.Name = name;
        this.Condition = condition;
    }


}


