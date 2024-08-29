using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct IsEqualsControlType(
    
    [property: Required] IUiElementFunction Element,
    [property: Required] ControlType ControlType,
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
        var left = ele.ControlType == this.ControlType;

        if(left == right)
        {
            return ele;
        }

        return null;
    }

}
