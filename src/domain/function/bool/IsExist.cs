using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


[Description("値が存在するか(= nullではないか)判定する")]
public readonly record struct IsExist : IBoolFunction
{
    [Required]
    public IAnyFunction Target { get; }

    

    [JsonConstructor]
    public IsExist(IAnyFunction target)
    {
        Target = target;
    }
    

    public async Task<bool> Call(IRuleContext ctx)
    {   
        return (await this.Target.CallDynamic<dynamic>(ctx)) != null;
    }
}
