

namespace NotifiableTools;

public class FunctionContext : IFunctionContext
{
    private Rule rule;
    private Dictionary<Type, IDisposable> pool = new Dictionary<Type, IDisposable>();



    public FunctionContext(Rule rule)
    {
        this.rule = rule;
    }

    public bool IsDebug()
    {
        return this.rule.IsDebug;
    }

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable
    {
        if(pool.TryGetValue(typeof(T), out IDisposable? value))
        {
            return (T)value;
        }

        var newValue = factory();
        pool.Add(typeof(T), newValue);
        return newValue;
    }

    
    public void Dispose()
    {
        this.pool.Values.ToList().ForEach((v) => v.Dispose());
    }

}