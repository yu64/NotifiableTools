


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
        Action<Rule> tellStart, 
        Action<Rule> tellStop, 
        CancellationTokenSource cts)
    {
        return this.usecase.ObserveRules(
            rules,
            tellStart,
            tellStop,
            cts
        );
    }

    internal void Execute(INotion notion, IDictionary<string, string> args)
    {
        System.Console.WriteLine(args);
    }
}