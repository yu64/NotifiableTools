
using System.Text.Json.Serialization;

namespace NotifiableTools;

public record class Auto : AbstractAction
{
    [JsonConstructor]
    public Auto(string name, string command) : base(name, command)
    {
    }
}