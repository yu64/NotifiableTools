
using System.Collections.Immutable;
using System.Windows;
using static NotifiableTools.PopupTool;

namespace NotifiableTools;

public class NotionController : IDisposable
{
    

    public delegate IDisposable ContextMenuRegister(string? text, Image? image, EventHandler? onClick);


    private readonly IActionExecutor executor;

    private Dictionary<Rule, ImmutableList<IDisposable>> ruleToDisposable = [];


    public NotionController(IActionExecutor executor)
    {
        this.executor = executor;
    }


    //======================================================================================


    public void ShowNotions(Rule rule, ContextMenuRegister register)
    {
        System.Console.WriteLine($"enable {rule.Name}");
        
        if(this.ruleToDisposable.ContainsKey(rule))
        {
            return;
        }

        //(UI)メインスレッドで同期実行する
        System.Windows.Application.Current.Dispatcher.Invoke(() => {

            var displayNotions = rule.Notions
                .Select((n) => this.ShowNotion(n, register))
                .Where((d) => d != null)
                .ToImmutableList();
                
            this.ruleToDisposable.Add(rule, displayNotions);
        });
    }

    private IDisposable ShowNotion(INotion notion, ContextMenuRegister register)
    {
        return notion switch
        {
            Button => new PopupTool(PopupType.Button, (args) => this.executor.Execute(notion, args)),
            
            _ => throw new ArgumentOutOfRangeException($"Notion:\"{notion.GetType().Name}\"に対応する表示方法が見つかりません"),
        };
    }
    





    //======================================================================================


    public void HideNotions(Rule rule)
    {
        System.Console.WriteLine($"disable {rule.Name}");
        
        if(!ruleToDisposable.TryGetValue(rule, out ImmutableList<IDisposable>? chlidren))
        {
            return;
        }

        this.ruleToDisposable.Remove(rule);

        //(UI)メインスレッドで同期実行する
        System.Windows.Application.Current.Dispatcher.Invoke(() => {

            chlidren.ForEach((w) => w.Dispose());
        });
    }


    public void Dispose()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() => {

            this.ruleToDisposable.Values
                .SelectMany((list) => list)
                .ToList()
                .ForEach((w) => w.Dispose());
        });
    }



    //======================================================================================



}