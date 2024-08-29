


using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;
using SmartFormat.Utilities;

namespace NotifiableTools;



[Description("指定された値をAction用の引数として保存します。Custom.Fooでアクセス。")]
public readonly record struct SaveBoolToActionArg (

    [property: Required] string Name,
    [property: Required] IBoolFunction Value

) : IBoolFunction, ISaveFunction<IBoolFunction, bool>;

[Description("指定された値をAction用の引数として保存します。Custom.Fooでアクセス。")]
public readonly record struct SaveStringToActionArg (

    [property: Required] string Name,
    [property: Required] IStringFunction Value

) : IStringFunction, ISaveFunction<IStringFunction, string>;

[Description("指定された値をAction用の引数として保存します。Custom.Fooでアクセス。プロパティ情報のみに加工されます。")]
public readonly record struct SaveElementToActionArg (

    [property: Required] string Name,
    [property: Required] IUiElementFunction Value

) : IUiElementFunction, ISaveFunction<IUiElementFunction, AutomationElement?>;

[Description("指定された値をAction用の引数として保存します。Custom.Fooでアクセス。")]
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
            AutomationElement ele => ISaveFunction<TFunc, TReturn>.ToDictonary(ele),
            _ => src
        };

        return Task.FromResult(src);
    }


    private static ImmutableDictionary<string, object?> ToDictonary(AutomationElement ele)
    {
        try
        {
            var prop = ele.Properties;
            var propType = prop.GetType();

            var dict = propType.GetProperties()
            .Where((p) => p.PropertyType.IsGenericType)
            .Where((p) => p.PropertyType.GetGenericTypeDefinition() == typeof(AutomationProperty<>))
            .Select((p) => (p.Name, (dynamic?)p.GetValue(prop)))
            .ToDictionary((t) => t.Name, (t) => (object?) t.Item2?.ValueOrDefault)
            .ToImmutableDictionary()
            ;

            return dict;
        }
        catch(COMException)
        {
            return ImmutableDictionary.Create<string, object?>();
        }
    }
}




