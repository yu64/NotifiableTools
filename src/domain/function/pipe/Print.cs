
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct PrintBool(

    [property: Description("SmartFormat")] string Format,
    [property: Required] IBoolFunction Value

) : IBoolFunction, IPrintFunction<IBoolFunction, bool>;

public readonly record struct PrintString(

    [property: Description("SmartFormat")] string Format,
    [property: Required] IStringFunction Value

) : IStringFunction, IPrintFunction<IStringFunction, string>;

public readonly record struct PrintElement(

    [property: Description("SmartFormat")] string Format,
    [property: Required] IUiElementFunction Value

) : IUiElementFunction, IPrintFunction<IUiElementFunction, AutomationElement?>;

public readonly record struct PrintProcess(

    [property: Description("SmartFormat")] string Format,
    [property: Required] IProcessFunction Value

) : IProcessFunction, IPrintFunction<IProcessFunction, Process?>;





interface IPrintFunction<TFunc, TReturn> : IPipeFunction<TFunc, TReturn> where TFunc : IAnyFunction<TReturn>
{
    public string Format { get; }

    Task<TReturn> IPipeFunction<TFunc, TReturn>.CallPipe(IRuleContext ctx, TReturn src)
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