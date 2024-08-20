
namespace NotifiableTools;

public interface IRuleSetContext : IDisposable
{

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;
    public R RunExclusively<T, R>(Func<T> factory, Func<T, R> action) where T : IDisposable;
    
}