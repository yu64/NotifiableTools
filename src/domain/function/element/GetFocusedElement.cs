using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct GetFocusedElement() : IUiElementFunction
{

    public Task<AutomationElement?> Call(IRuleContext ctx)
    {
        try
        {
            var ele = ctx.ruleSetContext.RunExclusively(
                () => new UIA3Automation(),
                (auto) => auto.FocusedElement()
            );
            
            ctx.SetVariable(this, ele);
            return Task.FromResult<AutomationElement?>(ele);
        }
        catch (COMException)
        {
            return Task.FromResult(ctx.GetVariable<AutomationElement?>(this));
        }
    }

}
