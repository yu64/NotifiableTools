using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Not : IBoolFunction
{
    [Required]
    public IBoolFunction Value { get; }


    [JsonConstructor]
    public Not(IBoolFunction value)
    {
        Value = value;
    }


    public async Task<bool> Call(IRuleContext ctx)
    {
        return !await Value.Call(ctx);
    }
}
