

using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;

public interface IPipeFunction<TFunc, TReturn> : IAnyFunction<TReturn> where TFunc : IAnyFunction<TReturn>
{
    public TFunc Value { get; }

    public Task<TReturn> CallPipe(IRuleContext ctx, TReturn value);
    

    async Task<TReturn> IAnyFunction<TReturn>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Value.CallDynamic<TReturn>(ctx));
    }
}



public interface IPipeFunction : IBoolFunction, IStringFunction, IUiElementFunction, IProcessFunction
{

    public IAnyFunction Value { get; }

    public Task<T> CallPipe<T>(IRuleContext ctx, T value);



    async Task<T> IAnyFunction.CallDynamic<T>(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Value.CallDynamic<T>(ctx));
    }

    async Task<bool> IAnyFunction<bool>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Value.CallDynamic<bool>(ctx));
    }

    async Task<string> IAnyFunction<string>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Value.CallDynamic<string>(ctx));
    }

    async Task<AutomationElement?> IAnyFunction<AutomationElement?>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Value.CallDynamic<AutomationElement>(ctx));
    }

    async Task<Process?> IAnyFunction<Process?>.Call(IRuleContext ctx)
    {
        return await this.CallPipe(ctx, await this.Value.CallDynamic<Process>(ctx));
    }
}