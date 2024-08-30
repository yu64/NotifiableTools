

using System.Collections.Immutable;

namespace NotifiableTools;

public class RuleContext : IRuleContext
{

    public IRuleSetContext ruleSetContext { get; }
    public IDictionary<string, object?> customArgs { get; }


    private readonly Rule rule;
    private readonly Dictionary<Type, IDisposable> commonPool = new();
    private readonly Dictionary<IAnyFunction, dynamic> localVariable = new();
    private readonly Stack<Scope> scopeStack = new();


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

    public T? GetVariable<T>(IAnyFunction key)
    {
        return this.localVariable.GetValueOrDefault(key);
    }

    public IDisposable OpenScope(IAnyFunction owner, object scopeValue)
    {
        Scope scope = new Scope(owner, scopeValue);
        this.scopeStack.Push(scope);
        return new DisposableWrapper(() => {

            var removedScope = this.scopeStack.Pop();
            if(scope != removedScope)
            {
                throw new Exception("スコープが不正な順序で閉じられました");
            }
        });
    }

    public ImmutableList<Scope> GetScopes()
    {
        return this.scopeStack.ToImmutableList();
    }


    public void Dispose()
    {
        this.commonPool.Values.ToList().ForEach((v) => v.Dispose());
    }

}