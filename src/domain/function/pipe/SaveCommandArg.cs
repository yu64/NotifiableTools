


using System.ComponentModel.DataAnnotations;

namespace NotifiableTools;

public readonly record struct SaveCommandArg (


    [property: Required] String Name,
    [property: Required] IAnyFunction Src

) : IFunctionPipe
{
    
    Task<T> IFunctionPipe.CallPipe<T>(IRuleContext ctx, T src)
    {
        return Task.FromResult(src);
    }
}