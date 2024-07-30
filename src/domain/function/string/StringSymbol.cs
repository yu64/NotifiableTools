using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct StringSymbol : IStringFunction
{
    [Required]
    public string Value { get; }


    [JsonConstructor]
    public StringSymbol(string value)
    {
        this.Value = value;
    }


    public Task<string> Call(IFunctionContext ctx)
    {
        return Task.FromResult(this.Value);
    }
}
