

using System.Text.Json.Serialization;

namespace NotifiableTools;

[AllSubType]
public abstract class AbstractAction
{
    public string Shell { get; }

    [JsonConstructor]
    public AbstractAction(string shell)
    {
        this.Shell = shell;
    }



}