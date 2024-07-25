
using System.Text.Json.Serialization;

namespace NotifiableTools;

public class Auto : AbstractAction
{
    [JsonConstructor]
    public Auto(string command) : base(command)
    {
    }
}