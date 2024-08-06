
using System.Windows;

namespace NotifiableTools;

public class NotionController : IDisposable
{
    private Dictionary<Rule, ImmutableList<IDisposable>> ruleToNotions = [];


    public NotionController()
    {
        
    }

    public void Dispose()
    {

    }

    public void ShowNotions(Rule rule, Func<string?, Image?, EventHandler?, IDisposable> registerMenuItem)
    {
        System.Console.WriteLine($"enable {rule.Name}");
        
        if(this.ruleToNotions.ContainsKey(rule))
        {
            return;
        }
        
        //(UI)メインスレッドで同期実行する
        System.Windows.Application.Current.Dispatcher.Invoke(() => {

            var children = rule.Notions.Select((v) => new NotionController(v, this.trayApp)).ToImmutableList();
            this.ruleToNotions.Add(rule, children);
        });
        

        
    }

    
    public void HideNotions(Rule rule)
    {
        System.Console.WriteLine($"disable {rule.Name}");
        
        if(!ruleToNotions.TryGetValue(rule, out ImmutableList<IDisposable>? chlidren))
        {
            return;
        }

        this.ruleToNotions.Remove(rule);

        //(UI)メインスレッドで同期実行する
        this.trayApp.Dispatcher.Invoke(() => {

            chlidren.ForEach((w) => w.Dispose());
        });
        
        
    }

}