

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using FlaUI.Core.AutomationElements;

namespace NotifiableTools;

public interface IFunctionPipe : IBoolFunction, IStringFunction, IUiElementFunction, IProcessFunction
{

    public IAnyFunction Src { get; }

    public Task<T> CallPipe<T>(IRuleContext ctx, T src);



    async Task<T> IAnyFunction.CallDynamic<T>(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Src.CallDynamic<T>(ctx));
    }

    async Task<bool> IAnyFunction<bool>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Src.CallDynamic<bool>(ctx));
    }

    async Task<string> IAnyFunction<string>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Src.CallDynamic<string>(ctx));
    }

    async Task<AutomationElement?> IAnyFunction<AutomationElement?>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Src.CallDynamic<AutomationElement>(ctx));
    }

    async Task<Process?> IAnyFunction<Process?>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Src.CallDynamic<Process>(ctx));
    }
}