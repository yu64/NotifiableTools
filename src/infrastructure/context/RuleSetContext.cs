

namespace NotifiableTools;

public class RuleSetContext : IRuleSetContext
{
    private readonly RuleSet ruleSet;

    public RuleSetContext(RuleSet ruleSet)
    {
        this.ruleSet = ruleSet;
    }


    public T GetCommonObject<T>()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

}