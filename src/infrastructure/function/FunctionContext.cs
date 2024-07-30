

namespace NotifiableTools;

public class FunctionContext : IFunctionContext
{
    private Rule rule;
    private Dictionary<Type, IDisposable> pool = new Dictionary<Type, IDisposable>();



    public FunctionContext(Rule rule)
    {
        this.rule = rule;
    }   

    public void Log(params object[] objs)
    {
        if(!this.rule.EnableLog)
        {
            return;
        }

        System.Console.WriteLine(String.Join(", ", objs.Select((o) => o.ToString())));
    }

    public void LogFuncResult(object retrunValue, Type func, params object[] values)
    {
        if(!this.rule.EnableLog)
        {
            return;
        }

        var returnText = $"<{retrunValue.GetType().Name}>{retrunValue}";
        var argTexts = values.Select((v) => $"<{v.GetType().Name}>{v}");
        var argLine = String.Join(", ", argTexts);

        System.Console.WriteLine($"{returnText} = {func.Name}({argLine})");
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