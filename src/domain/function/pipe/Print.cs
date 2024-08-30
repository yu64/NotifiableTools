
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





file interface IPrintFunction<TFunc, TReturn> : IAnyFunction<TReturn> where TFunc : IAnyFunction<TReturn>
{
    public string Format { get; }
    public TFunc Value { get; }

    async Task<TReturn> IAnyFunction<TReturn>.Call(IRuleContext ctx)
    {
        var src = await this.Value.CallDynamic<TReturn>(ctx);
        
        if(String.IsNullOrWhiteSpace(this.Format))
        {
            System.Console.WriteLine(src);
            return src;
        }

        try
        {
            var text = SmartFormatUtil.Format(this.Format, src);
            System.Console.WriteLine(text);
            return src;
        }
        catch(Exception ex)
        {
            SmartFormatUtil.PrintErrorMessage(ex, this.Format, src);
            return src;
        }
    }

}