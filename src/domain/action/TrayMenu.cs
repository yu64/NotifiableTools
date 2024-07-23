
using System.Text.Json.Serialization;

namespace NotifiableTools;

public class TrayMenu : AbstractAction
{
    [JsonConstructor]
    public TrayMenu(string shell) : base(shell)
    {
    }
}