using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Rule
{
    [Required]
    public string Name { get; }

    [Required]
    public IBoolFunction Condition { get; }

    [JsonConstructor]
    public Rule(string name, IBoolFunction condition)
    {
        this.Name = name;
        this.Condition = condition;
    }


}


