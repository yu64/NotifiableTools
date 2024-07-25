

using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

[AllSubType]
public abstract class AbstractAction
{
    [Required]
    public string Command { get; }

    [JsonConstructor]
    public AbstractAction(string command)
    {
        this.Command = command;
    }


}