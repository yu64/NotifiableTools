
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public class HookedInput : AbstractAction
{
    [Required]
    public IPosition Position { get; }
    
    [JsonConstructor]
    public HookedInput(string command, IPosition position) : base(command)
    {
        this.Position = position;
    }
}