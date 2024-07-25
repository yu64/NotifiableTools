using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsContain : IBoolFunction
{

    [Required]
    public string All { get; }
    
    [Required]
    public string Part { get; }

    [JsonConstructor]
    public IsContain(string all, string part)
    {
        All = all;
        Part = part;
    }

    public Task<bool> Call()
    {
        return Task.FromResult(this.All.Contains(this.Part));
    }

}
