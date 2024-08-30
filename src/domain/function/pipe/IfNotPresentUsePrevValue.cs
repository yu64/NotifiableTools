
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





file interface IBaseFunction<TFunc, TReturn> : IAnyFunction<TReturn> where TFunc : IAnyFunction<TReturn>
{
    public TFunc Value { get; }

    Task<TReturn> IAnyFunction<TReturn>.Call(IRuleContext ctx)
    {
        var src = this.Value.CallDynamic<TReturn>(ctx);

        if(src == null)
        {
            return Task.FromResult(ctx.GetVariable<TReturn>(this)!);
        }

        ctx.SetVariable(this, src);
        return src;
    }

}