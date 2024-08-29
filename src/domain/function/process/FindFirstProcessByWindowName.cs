using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct FindFirstProcessByWindowName(

    [property: Required] IStringFunction Name,
    [property: Default(true)] bool CanCheckSubString

) : IProcessFunction
{

    public async Task<Process?> Call(IRuleContext ctx)
    {
        var name = await this.Name.Call(ctx);
        var canCheckSubString = this.CanCheckSubString;

        return Process.GetProcesses()
            .Where((p) => (!canCheckSubString || p.MainWindowTitle.Contains(name)))
            .Where((p) => (canCheckSubString || String.Equals(p.MainWindowTitle, name)))
            .FirstOrDefault()
            ;
    }
}
