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
    [property: Required] ControlType ControlType


) : IUiElementFunction
{


    public async Task<AutomationElement?> Call(IFunctionContext ctx)
    {
        var ele = await this.Element.Call(ctx);

        if(ele != null && ele.ControlType == this.ControlType)
        {
            return ele;
        }

        return null;
    }

}
