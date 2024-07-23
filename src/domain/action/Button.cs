
using System.Text.Json.Serialization;

namespace NotifiableTools;

public class Button : AbstractAction
{
    [JsonConstructor]
    public Button(string shell) : base(shell)
    {
    }
}