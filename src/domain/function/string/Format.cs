using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct Format (

    [property: Description("SmartFormat")] string Template,
    [property: Required] IAnyFunction Value

) : IStringFunction
{

    public async Task<string> Call(IRuleContext ctx)
    {
        var arg = await this.Value.CallDynamic<dynamic>(ctx);

        if(String.IsNullOrWhiteSpace(this.Template))
        {
            return Task.FromResult(arg.ToString());
        }

        try
        {
            var text = SmartFormatUtil.Format(this.Template, arg);
            return Task.FromResult(text);
        }
        catch(Exception ex)
        {
            SmartFormatUtil.PrintErrorMessage(ex, this.Template, arg);
            return Task.FromResult(arg.ToString());
        }
    }
}
