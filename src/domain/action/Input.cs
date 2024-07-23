
using System.Text.Json.Serialization;

namespace NotifiableTools;

public class Input : AbstractAction
{
    [JsonConstructor]
    public Input(string shell) : base(shell)
    {
    }
}