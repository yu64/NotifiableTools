using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct BoolSymbol : IBoolFunction
{   
    [Default(false)]
    public bool Value { get; }


    [JsonConstructor]
    public BoolSymbol(bool value)
    {
        this.Value = value;
    }


    public Task<bool> Call(IFunctionContext ctx)
    {
        return Task.FromResult(this.Value);
    }
}
