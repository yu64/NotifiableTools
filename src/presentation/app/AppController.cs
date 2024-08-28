


namespace NotifiableTools;


public class AppController
{
    private readonly Usecase usecase;


    public AppController(
        Usecase usecase
    )
    {
        this.usecase = usecase;
    }

    public CancellationTokenSource ObserveRules(
        RuleSet rules, 
        Action<IRuleContext, Rule> tellStart, 
        Action<IRuleContext, Rule> tellStop, 
        CancellationTokenSource cts)
    {
        return this.usecase.ObserveRules(
            rules,
            tellStart,
            tellStop,
            cts
        );
    }

    internal void Execute(ActionArgs args)
    {
        usecase.Execute(args);
    }
}