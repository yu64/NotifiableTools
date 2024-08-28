


using Json.Schema.Generation;
using SmartFormat;
using SmartFormat.Extensions;

namespace NotifiableTools;

public readonly record struct Print (

    [property: Nullable(true)] string Format,
    [property: Required] IAnyFunction Src

) : IFunctionPipe
{
    
    Task<T> IFunctionPipe.CallPipe<T>(IRuleContext ctx, T src)
    {
        if(String.IsNullOrWhiteSpace(this.Format))
        {
            System.Console.WriteLine(src);
            return Task.FromResult(src);
        }

        try
        {
            var text = SmartFormatUtil.Format(this.Format, src);
            System.Console.WriteLine(text);
            return Task.FromResult(src);
        }
        catch(Exception ex)
        {
            SmartFormatUtil.PrintErrorMessage(ex, this.Format, src);
            return Task.FromResult(src);
        }
    }
}