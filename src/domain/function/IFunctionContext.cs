
namespace NotifiableTools;

public interface IFunctionContext : IDisposable
{

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;


}