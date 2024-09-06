using System.CommandLine.Parsing;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Toggle (

    [property: Required] IBoolFunction CanToggle,
    [property: Default(false)] bool DefaultValue = false,

    [property: Description("Debug message prefix name. if empty, no output.")]
    [property: Default("")] 
    string DebugName = ""

) : IBoolFunction
{


    public async Task<bool> Call(IRuleContext ctx)
    {
        var prev = ctx.GetVariable<bool?>(this) ?? DefaultValue;

        var canToggle = await this.CanToggle.Call(ctx);
        if(!canToggle)
        {
            //切り替えられない場合、そのまま出力
            this.print(prev, prev, canToggle);
            return prev;
        }

        //切り替えて保存
        var next = !prev;
        ctx.SetVariable(this, next);
        this.print(prev, next, canToggle);
        return next;
    }

    private void print(bool prev, bool next, bool canToggle)
    {
        if(String.IsNullOrEmpty(this.DebugName))
        {
            return;
        }

        System.Console.WriteLine($"[{nameof(Toggle)}:{this.DebugName}] Prev:{prev}, Next:{next}, CanToggle:{canToggle}");
    }
}
