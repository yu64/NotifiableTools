

namespace NotifiableTools;

public class RuleContext : IRuleContext
{

    public IRuleSetContext ruleSetContext { get; }

    private readonly Rule rule;
    private readonly Dictionary<Type, IDisposable> commonPool = new();


    public RuleContext(IRuleSetContext ruleSetContext, Rule rule)
    {
        this.ruleSetContext = ruleSetContext;
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