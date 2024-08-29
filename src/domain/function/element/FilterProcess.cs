using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct FilterProcess(
    
    [property: Required] IUiElementFunction Element,
    [property: Required] IProcessFunction Process,
    [property: Default(false)] bool CanExclude = false



) : IUiElementFunction
{


    public async Task<AutomationElement?> Call(IRuleContext ctx)
    {
        var ele = await this.Element.Call(ctx);
        if(ele == null)
        {
            return null;
        }

        var process = await this.Process.Call(ctx);

        var right = !this.CanExclude;
        var left = ele.Properties.ProcessId == process?.Id;

        if(left == right)
        {
            return ele;
        }

        return null;
    }
}
