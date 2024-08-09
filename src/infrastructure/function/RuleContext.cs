

namespace NotifiableTools;

public class RuleContext : IRuleContext
{
    private Rule rule;
    private Dictionary<Type, IDisposable> commonPool = new();
    private Dictionary<(IAnyFunction, Type), IDisposable> ownerPool = new();


    public RuleContext(Rule rule)
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
        this.commonPool.Values.ToList().ForEach((v) => v.Dispose());
    }

}