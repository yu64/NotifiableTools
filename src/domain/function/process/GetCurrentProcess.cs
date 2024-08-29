using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct GetCurrentProcess() : IProcessFunction
{

    public Task<Process?> Call(IRuleContext ctx)
    {
        return Task.FromResult<Process?>(Process.GetCurrentProcess());
    }
}
