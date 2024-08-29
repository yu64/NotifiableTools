using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct FilterAutomationId(
    
    [property: Required] IStringFunction AutomationId,
    [property: Required] IUiElementFunction Element,
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

        var right = !this.CanExclude;
        var left = string.Equals(ele.Properties.AutomationId.ValueOrDefault, await this.AutomationId.Call(ctx));

        if(left == right)
        {
            return ele;
        }

        return null;
    }
}
