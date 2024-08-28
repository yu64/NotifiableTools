


using System.ComponentModel.DataAnnotations;

namespace NotifiableTools;

public readonly record struct SaveActionArg (


    [property: Required] string Name,
    [property: Required] IAnyFunction Src

) : IFunctionPipe
{
    
    Task<T> IFunctionPipe.CallPipe<T>(IRuleContext ctx, T src)
    {
        ctx.customArgs[this.Name] = src;
        return Task.FromResult(src);
    }
}