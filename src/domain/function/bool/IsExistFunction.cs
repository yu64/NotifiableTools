using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsExistFunction : IBoolFunction
{
    [Required]
    public IAnyFunction Target { get; }

    

    [JsonConstructor]
    public IsExistFunction(IAnyFunction target)
    {
        Target = target;
    }
    

    public bool Call()
    {
        ICallableFunction<dynamic> callable = (ICallableFunction<dynamic>)Target;
        return (callable?.Call() != null);
    }
}
