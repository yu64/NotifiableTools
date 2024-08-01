
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class HookedInput : AbstractAction
{

    
    [JsonConstructor]
    public HookedInput(string name, string command) : base(name, command)
    {

    }
}