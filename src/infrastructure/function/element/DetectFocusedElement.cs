using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct DetectFocusedElement() : IUiElementFunction
{

    public async Task<AutomationElement?> Call(IFunctionContext ctx)
    {
        
        var auto = ctx.GetOrCreateDisposable(() => new UIA3Automation());
        var tcs = new TaskCompletionSource<AutomationElement>();

        var handlerId = auto.RegisterFocusChangedEvent((ele) => {

            
            tcs.SetResult(ele);
        });

        var result = await tcs.Task;

        auto.UnregisterFocusChangedEvent(handlerId);

        return result;
    }

}
