using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Not : IBoolFunction
{
    [Required]
    public IBoolFunction Target { get; }


    [JsonConstructor]
    public Not(IBoolFunction target)
    {
        Target = target;
    }


    public async Task<bool> Call(IFunctionContext ctx)
    {
        return !await Target.Call(ctx);
    }
}
