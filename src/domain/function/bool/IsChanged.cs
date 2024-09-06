using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core.AutomationElements;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsChanged(

    [property: Required] IAnyFunction Value,
    [property: Default(false)] bool CanIgoneNull = false
    

) : IBoolFunction
{


    public async Task<bool> Call(IRuleContext ctx)
    {
        var old = ctx.GetVariable<dynamic>(this);
        var now = await Value.CallDynamic<dynamic>(ctx);


        //パターン
        //old=hoge, now=hoge => var=hoge, result=false
        //old=hoge, now=fuga => var=fuga, result=true
        //old=null, now=hoge => var=hoge, result=false
        //old=hoge, now=null => var=hoge, result=false
        //old=null, now=null => var=null, result=false

        //nullを無視するモードかつ、nullを含んでいる場合
        if(this.CanIgoneNull && (old == null || now == null))
        {
            //新しい値を優先しつつ、nullではないほうを取得する
            ctx.SetVariable(this, now ?? old);

            //nullを無視しているため、常にfalse
            return false;
        }

        ctx.SetVariable(this, now);
        return !this.isEquals(old, now);
    }

    private bool isEquals(dynamic old, dynamic now)
    {
        if(old == null && now == null)
        {
            return true;
        }

        if(old == null)
        {
            return false;
        }

        if(now == null)
        {
            return false;
        }


        return (old, now) switch
        {   
            (AutomationElement o, AutomationElement n) => (
                o.Properties.AutomationId.ValueOrDefault == n.Properties.AutomationId.ValueOrDefault
                && o.Properties.ProcessId.ValueOrDefault == n.Properties.ProcessId.ValueOrDefault
                && o.Properties.ControlType.ValueOrDefault == n.Properties.ControlType.ValueOrDefault
                && o.Properties.BoundingRectangle.ValueOrDefault == n.Properties.BoundingRectangle.ValueOrDefault
                && o.Properties.FrameworkId.ValueOrDefault == n.Properties.FrameworkId.ValueOrDefault
            ),
            (Process o, Process n) => (
                o.ProcessName == n.ProcessName
                && o.Id == n.Id
            ),
            _ => old == now
        };
    }
}
