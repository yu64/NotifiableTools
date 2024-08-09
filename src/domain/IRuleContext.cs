
namespace NotifiableTools;

public interface IRuleContext : IDisposable
{

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;


}