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

    public Task<AutomationElement?> Call(IRuleContext ctx)
    {
        var ele = ctx.ruleSetContext.RunExclusively(
            () => new UIA3Automation(),
            (auto) => auto.FocusedElement()
        );
        

        return Task.FromResult<AutomationElement?>(ele);
    }

}
