using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct And (

    [property: Required] IBoolFunction Left,
    [property: Required] IBoolFunction Right

) : IBoolFunction
{


    public async Task<bool> Call(IRuleContext ctx)
    {
        return (await this.Left.Call(ctx)) && (await this.Right.Call(ctx));
    }
}
