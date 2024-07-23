
using System.Text.Json.Serialization;

namespace NotifiableTools;

public class HookedInput : AbstractAction
{
    [JsonConstructor]
    public HookedInput(string shell) : base(shell)
    {
    }
}