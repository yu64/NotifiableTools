using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;


[AllSubType()]
public interface IAnyFunction
{
    [Required]
    public string Type { get; }
}

[AllSubType()]
public interface ICallableFunction<TResult> : IAnyFunction
{
    public TResult Call();
}

[AllSubType()]
public interface IBoolFunction : ICallableFunction<bool>
{

}

[AllSubType()]
public interface IUiElementFunction : ICallableFunction<AutomationElement?>
{

}

[AllSubType()]
public interface IProcessFunction : ICallableFunction<Process?>
{

}
