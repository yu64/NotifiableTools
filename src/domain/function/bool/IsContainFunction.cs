using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsContainFunction : IBoolFunction
{

    [Required]
    public string All { get; }
    
    [Required]
    public string Part { get; }

    [JsonConstructor]
    public IsContainFunction(string all, string part)
    {
        All = all;
        Part = part;
    }

    public bool Call()
    {
        return this.All.Contains(this.Part);
    }
}
