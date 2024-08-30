
using Json.Schema.Generation;
using NotifiableTools;

namespace NotifiableTools;

public readonly record struct GetLocalValue(

    [property: Required] string Name,
    IAnyFunction Default

) : IAllFunction
{
    public async Task<T> Call<T>(IRuleContext ctx)
    {
        var scope = ctx.GetScopes()
        .Where((s) => s.OwnerType == typeof(FunctionBlock))
        .FirstOrDefault();

        if(scope == null)
        {
            return default!;
        }

        var results = (Dictionary<string, object>)scope.Value;
        
        dynamic? result = results.GetValueOrDefault(this.Name);
        if(result != null)
        {
            return (T)result;
        }

        if(this.Default != null)
        {
            return await this.Default.CallDynamic<T>(ctx);
        }

        return default!;
    }
}