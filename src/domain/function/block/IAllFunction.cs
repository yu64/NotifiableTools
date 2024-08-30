

using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;




public interface IAllFunction : IBoolFunction, IStringFunction, IUiElementFunction, IProcessFunction
{


    public Task<T> Call<T>(IRuleContext ctx);


    async Task<T> IAnyFunction.CallDynamic<T>(IRuleContext ctx)
    {
        return await this.Call<T>(ctx);
    }

    async Task<bool> IAnyFunction<bool>.Call(IRuleContext ctx)
    {
        return await this.Call<bool>(ctx);
    }

    async Task<string> IAnyFunction<string>.Call(IRuleContext ctx)
    {
        return await this.Call<string>(ctx);
    }

    async Task<AutomationElement?> IAnyFunction<AutomationElement?>.Call(IRuleContext ctx)
    {
        return await this.Call<AutomationElement?>(ctx);
    }

    async Task<Process?> IAnyFunction<Process?>.Call(IRuleContext ctx)
    {
        return await this.Call<Process?>(ctx);
    }
}