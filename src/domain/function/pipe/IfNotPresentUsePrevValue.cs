
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IfNotPresentUsePrevBool(

    [property: Required] IBoolFunction Value

) : IBoolFunction, IBaseFunction<IBoolFunction, bool>;

public readonly record struct IfNotPresentUsePrevString(

    [property: Required] IStringFunction Value

) : IStringFunction, IBaseFunction<IStringFunction, string>;

public readonly record struct IfNotPresentUsePrevElement(

    [property: Required] IUiElementFunction Value

) : IUiElementFunction, IBaseFunction<IUiElementFunction, AutomationElement?>;

public readonly record struct IfNotPresentUsePrevProcess(

    [property: Required] IProcessFunction Value

) : IProcessFunction, IBaseFunction<IProcessFunction, Process?>;





file interface IBaseFunction<TFunc, TReturn> : IPipeFunction<TFunc, TReturn> where TFunc : IAnyFunction<TReturn>
{
    Task<TReturn> IPipeFunction<TFunc, TReturn>.CallPipe(IRuleContext ctx, TReturn src)
    {
        if(src == null)
        {
            return Task.FromResult(ctx.GetVariable<TReturn>(this)!);
        }

        ctx.SetVariable(this, src);
        return Task.FromResult(src);
    }

}