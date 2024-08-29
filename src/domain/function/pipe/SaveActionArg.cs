


using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;



public readonly record struct SaveBoolToActionArg (

    [property: Required] string Name,
    [property: Required] IBoolFunction Value

) : IBoolFunction, ISaveFunction<IBoolFunction, bool>;

public readonly record struct SaveStringToActionArg (

    [property: Required] string Name,
    [property: Required] IStringFunction Value

) : IStringFunction, ISaveFunction<IStringFunction, string>;

public readonly record struct SaveElementToActionArg (

    [property: Required] string Name,
    [property: Required] IUiElementFunction Value

) : IUiElementFunction, ISaveFunction<IUiElementFunction, AutomationElement?>;

public readonly record struct SaveProcessToActionArg (

    [property: Required] string Name,
    [property: Required] IProcessFunction Value

) : IProcessFunction, ISaveFunction<IProcessFunction, Process?>;





file interface ISaveFunction<TFunc, TReturn> : IPipeFunction<TFunc, TReturn> where TFunc : IAnyFunction<TReturn>
{
    public string Name { get; }

    Task<TReturn> IPipeFunction<TFunc, TReturn>.CallPipe(IRuleContext ctx, TReturn src)
    {
        ctx.customArgs[this.Name] = src switch
        {
            AutomationElement ele => ele.Properties,
            _ => src
        };

        return Task.FromResult(src);
    }

}




