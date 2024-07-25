
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public class Input : AbstractAction
{
    [Required]
    public IPosition Position { get; }
    
    [JsonConstructor]
    public Input(string command, IPosition position) : base(command)
    {
        this.Position = position;
    }
}