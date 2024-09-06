using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsChangedToBool(

    [property: Required] IBoolFunction Value,

    [property: Description("Returns true if the changed values ​​match.")]
    [property: Required]
    [property: Nullable(true)]
    bool ToValue,

    [property: Default(false)] bool DefaultValue = false


) : IBoolFunction
{


    public async Task<bool> Call(IRuleContext ctx)
    {
        var old = ctx.GetVariable<bool?>(this) ?? this.DefaultValue;
        var now = await Value.Call(ctx);

        ctx.SetVariable(this, now);
        return old != now && (this.ToValue == now);
    }

}
