using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;

[AllSubType()]
public interface IAnyFunction
{

    public Task<T> CallDynamic<T>(IRuleContext ctx)
    {
        throw new Exception();
    }

}

[AllSubType()]
public interface IAnyFunction<TResult> : IAnyFunction
{
    async Task<T> IAnyFunction.CallDynamic<T>(IRuleContext ctx)
    {
        //実行
        object? value = (await this.Call(ctx));

        //キャストして返す
#pragma warning disable 
        return (T)value;
#pragma warning restore
    }

    public Task<TResult> Call(IRuleContext ctx);
}



[AllSubType()]
public interface IBoolFunction : IAnyFunction<bool>;

[AllSubType()]
public interface IStringFunction : IAnyFunction<string>;

[AllSubType()]
public interface IUiElementFunction : IAnyFunction<AutomationElement?>;

[AllSubType()]
public interface IProcessFunction : IAnyFunction<Process?>;

