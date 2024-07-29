

using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

[AllSubType]
public abstract record class AbstractAction
{
    [Required]
    public string Name { get; }

    [Required]
    public string Command { get; }

    [JsonConstructor]
    public AbstractAction(string name, string command)
    {
        this.Name = name;
        this.Command = command;
    }


}