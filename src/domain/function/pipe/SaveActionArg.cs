


using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;

[Description("[副作用] 指定された値をActionの引数(Custom配下)として保存する。AutomationElementは加工される")]
public readonly record struct SaveActionArg (


    [property: Required] string Name,
    [property: Required] IAnyFunction Src,

    [property: Nullable(true)] 
    [property: Description("戻り値")]
    IAnyFunction Output


) : IFunctionPipe
{
    
    Task<T> IFunctionPipe.CallPipe<T>(IRuleContext ctx, T src)
    {
        ctx.customArgs[this.Name] = src switch
        {
            AutomationElement ele => ele.Properties,
            _ => src
        };
        
        if(this.Output != null)
        {
            return this.Output.CallDynamic<T>(ctx);
        }

        return Task.FromResult(src);
    }
}