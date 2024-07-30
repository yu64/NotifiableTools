

namespace NotifiableTools;

public class FunctionContext : IFunctionContext
{
    private Rule rule;
    private Dictionary<Type, IDisposable> commonPool = new();
    private Dictionary<(IAnyFunction, Type), IDisposable> ownerPool = new();


    public FunctionContext(Rule rule)
    {
        this.rule = rule;
    }   

    

    public T GetOrCreateDisposable<T>(Func<T> factory) where T : IDisposable
    {
        if(commonPool.TryGetValue(typeof(T), out IDisposable? value))
        {
            return (T)value;
        }

        var newValue = factory();
        commonPool.Add(typeof(T), newValue);
        return newValue;
    }


    public void Dispose()
    {
        this.ownerPool.Values.ToList().ForEach((v) => v.Dispose());
    }

}