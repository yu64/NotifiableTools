using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct TrueSymbol : IBoolFunction
{
    [JsonConstructor]
    public TrueSymbol()
    {

    }


    public Task<bool> Call()
    {
        return Task.FromResult(true);
    }
}
