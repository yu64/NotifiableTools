using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


[Description("値が存在するか(= nullではないか)判定する")]
public readonly record struct IsExist : IBoolFunction
{
    [Required]
    public IAnyFunction Value { get; }

    

    [JsonConstructor]
    public IsExist(IAnyFunction value)
    {
        Value = value;
    }
    

    public async Task<bool> Call(IRuleContext ctx)
    {   
        return (await this.Value.CallDynamic<dynamic>(ctx)) != null;
    }
}
