
namespace NotifiableTools;

public interface IRuleSetContext : IDisposable
{

    public T GetCommonObject<T>();

}