

namespace NotifiableTools;

public class RuleContext : IRuleContext
{

    public IRuleSetContext ruleSetContext { get; }
    public IDictionary<string, object?> customArgs { get; }


    private readonly Rule rule;
    private readonly Dictionary<Type, IDisposable> commonPool = new();
    private readonly Dictionary<IAnyFunction, dynamic> localVariable = new();


    public RuleContext(IRuleSetContext ruleSetContext, Rule rule)
    {
        this.ruleSetContext = ruleSetContext;
        this.rule = rule;
        this.customArgs = new Dictionary<string, object?>();
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

    public void SetVariable(IAnyFunction key, object value)
    {
        this.localVariable[key] = value;
    }

    public T GetVariable<T>(IAnyFunction key)
    {
        return this.localVariable[key];
    }


    public void Dispose()
    {
        this.commonPool.Values.ToList().ForEach((v) => v.Dispose());
    }

}