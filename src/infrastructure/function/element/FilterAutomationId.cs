using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct FilterAutomationId(
    
    [property: Required] IUiElementFunction Element,
    [property: Required] IStringFunction AutomationId


) : IUiElementFunction
{


    public async Task<AutomationElement?> Call(IFunctionContext ctx)
    {
        var ele = await this.Element.Call(ctx);

        if(ele != null && String.Equals(ele.Properties.AutomationId.ValueOrDefault, this.AutomationId))
        {
            return ele;
        }

        return null;
    }
}
