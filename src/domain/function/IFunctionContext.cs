
namespace NotifiableTools;

public interface IFunctionContext : IDisposable
{

    public bool IsDebug();

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;

    
}