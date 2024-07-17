using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public class NotFunction : IBoolFunction
{
    [Required]
    [Const(nameof(NotFunction))]
    public string Type => nameof(NotFunction);

    [Required]
    public required IBoolFunction Target;

    public bool Call()
    {
        return !Target.Call();
    }
}
