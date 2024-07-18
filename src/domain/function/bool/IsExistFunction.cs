using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public class IsExistFunction : IBoolFunction
{
    [Required]
    public required IAnyFunction Target;

    public bool Call()
    {
        ICallableFunction<dynamic> callable = (ICallableFunction<dynamic>)Target;
        return (callable?.Call() != null);
    }
}
