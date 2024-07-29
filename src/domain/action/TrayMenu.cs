
using System.Text.Json.Serialization;

namespace NotifiableTools;

public record class TrayMenu : AbstractAction
{
    [JsonConstructor]
    public TrayMenu(string name, string command) : base(name, command)
    {
    }
}