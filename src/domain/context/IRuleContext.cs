
namespace NotifiableTools;

public interface IRuleContext : IDisposable
{
    public IRuleSetContext ruleSetContext { get; }

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;


}