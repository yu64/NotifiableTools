using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct GetFocusedElement() : IUiElementFunction
{

    public Task<AutomationElement?> Call(IFunctionContext ctx)
    {
        var auto = ctx.GetOrCreateDisposable(() => new UIA3Automation());
        var ele = auto.FocusedElement();

        return Task.FromResult<AutomationElement?>(ele);
    }

}
