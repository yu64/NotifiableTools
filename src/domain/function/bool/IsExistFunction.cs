using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public class IsExistFunction : IBoolFunction
{
    [Required]
    [Const(nameof(IsExistFunction))]
    public string Type => nameof(IsExistFunction);

    [Required]
    public required IAnyFunction Target;

    public bool Call()
    {
        ICallableFunction<dynamic> callable = (ICallableFunction<dynamic>)Target;
        return (callable?.Call() != null);
    }
}
