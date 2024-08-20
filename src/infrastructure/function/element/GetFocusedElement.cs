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
        

        // 自分自身のプロセスに関係するものを除外
        if(ele.Properties.ProcessId.ValueOrDefault == Environment.ProcessId)
        {
            return Task.FromResult<AutomationElement?>(null);
        }

        return Task.FromResult<AutomationElement?>(ele);
    }

}
