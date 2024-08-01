

using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

[AllSubType]
public abstract record class AbstractUiAction : AbstractAction
{
    [Required]
    public IPosition Position { get; }

    [JsonConstructor]
    public AbstractUiAction(string? name, string command, IPosition position) : base(name, command)
    {
        this.Position = position;
    }


}