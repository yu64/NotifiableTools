
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class Input : AbstractUiAction
{
    
    [JsonConstructor]
    public Input(string name, string command, IPosition position) : base(name, command, position)
    {
        
    }
}