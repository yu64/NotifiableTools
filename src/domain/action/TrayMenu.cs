
using System.Text.Json.Serialization;

namespace NotifiableTools;

public class TrayMenu : AbstractAction
{
    [JsonConstructor]
    public TrayMenu(string command) : base(command)
    {
    }
}