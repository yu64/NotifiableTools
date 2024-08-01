
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class Button : AbstractUiAction
{
    [JsonConstructor]
    public Button(string name, string command, IPosition position) : base(name, command, position)
    {

    }
}