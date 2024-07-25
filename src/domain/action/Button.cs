
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public class Button : AbstractAction
{

    [Required]
    public IPosition Position { get; }

    [JsonConstructor]
    public Button(string command, IPosition position) : base(command)
    {
        this.Position = position;
    }
}