
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class Button : AbstractAction
{

    [Required]
    public IPosition Position { get; }

    [JsonConstructor]
    public Button(string name, string command, IPosition position) : base(name, command)
    {
        this.Position = position;
    }
}