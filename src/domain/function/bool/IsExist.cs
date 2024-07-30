using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsExist : IBoolFunction
{
    [Required]
    public IAnyFunction Target { get; }

    

    [JsonConstructor]
    public IsExist(IAnyFunction target)
    {
        Target = target;
    }
    

    public async Task<bool> Call(IFunctionContext ctx)
    {   
        return (await this.Target.Call<dynamic>(ctx)) != null;
    }
}
