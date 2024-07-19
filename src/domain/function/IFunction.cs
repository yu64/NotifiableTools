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

    public Task<T> Call<T>()
    {
        throw new Exception();
    }

}

[AllSubType()]
public interface IAnyFunction<TResult> : IAnyFunction
{
    async Task<T> IAnyFunction.Call<T>()
    {
        //実行
        object? value = (await this.Call());

        //キャストして返す
#pragma warning disable 
        return (T)value;
#pragma warning restore
    }

    public Task<TResult> Call();
}

[AllSubType()]
public interface IBoolFunction : IAnyFunction<bool>
{

}

[AllSubType()]
public interface IUiElementFunction : IAnyFunction<AutomationElement?>
{

}

[AllSubType()]
public interface IProcessFunction : IAnyFunction<Process?>
{

}
