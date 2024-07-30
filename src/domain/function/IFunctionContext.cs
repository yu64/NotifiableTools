
namespace NotifiableTools;

public interface IFunctionContext : IDisposable
{

    public void Log(params object[] objs);

    public void LogFuncResult(object retrunValue, Type func, params object[] values);

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable;

    
}