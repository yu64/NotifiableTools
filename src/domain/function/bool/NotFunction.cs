using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public class NotFunction : IBoolFunction
{
    [Required]
    public required IBoolFunction Target;

    public bool Call()
    {
        return !Target.Call();
    }
}
