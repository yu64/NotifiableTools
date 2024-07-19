using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct NotFunction : IBoolFunction
{
    [Required]
    public IBoolFunction Target { get; }


    [JsonConstructor]
    public NotFunction(IBoolFunction target)
    {
        Target = target;
    }


    public async Task<bool> Call()
    {
        return !await Target.Call();
    }
}
