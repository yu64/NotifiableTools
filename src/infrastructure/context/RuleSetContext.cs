

using System.Security.Cryptography.X509Certificates;
using FlaUI.UIA3;
using Microsoft.VisualBasic.Logging;

namespace NotifiableTools;

public class RuleSetContext : IRuleSetContext
{
    private readonly RuleSet ruleSet;
    private readonly Dictionary<Type, IDisposable> commonPool = new();

    public RuleSetContext(RuleSet ruleSet)
    {
        this.ruleSet = ruleSet;

        this.GetOrCreateDisposable(() => new UIA3Automation());
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

    public R RunExclusively<T, R>(Func<T> factory, Func<T, R> action) where T : IDisposable
    {
        var value = this.GetOrCreateDisposable(factory);

        lock(value)
        {
            var result = action(value);
            return result;
        }
    }


    public void Dispose()
    {
        this.commonPool.Values.ToList().ForEach((v) => v.Dispose());
    }

}