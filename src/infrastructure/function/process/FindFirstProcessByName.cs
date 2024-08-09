using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct FindFirstProcessByName(

    [property: Required] IStringFunction Name

) : IProcessFunction
{

    public async Task<Process?> Call(IRuleContext ctx)
    {
        return Process.GetProcessesByName(await this.Name.Call(ctx)).FirstOrDefault();
    }
}
