
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class Input : AbstractAction
{
    [Required]
    public IPosition Position { get; }
    
    [JsonConstructor]
    public Input(string name, string command, IPosition position) : base(name, command)
    {
        this.Position = position;
    }
}