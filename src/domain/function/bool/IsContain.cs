using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsContain : IBoolFunction
{

    [Required]
    public IStringFunction All { get; }
    
    [Required]
    public IStringFunction Part { get; }

    [JsonConstructor]
    public IsContain(IStringFunction all, IStringFunction part)
    {
        All = all;
        Part = part;
    }

    public async Task<bool> Call(IRuleContext ctx)
    {
        return (await this.All.Call(ctx)).Contains(await this.Part.Call(ctx));
    }

}
