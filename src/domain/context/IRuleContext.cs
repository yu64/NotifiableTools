
namespace NotifiableTools;

public interface IRuleContext : IDisposable
{
    public IRuleSetContext ruleSetContext { get; }

    public IDictionary<string, object?> customArgs { get; }

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;

    public void SetVariable(IAnyFunction key, object value);
    public T? GetVariable<T>(IAnyFunction key); 

}