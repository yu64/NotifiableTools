

using System.Collections.Concurrent;

namespace NotifiableTools;


public class AppView
{
    private readonly AppController controller;

    private readonly CancellationTokenSource cts;

    private readonly IDictionary<INotion, IDisposable> showedNotions = new ConcurrentDictionary<INotion, IDisposable>();

    private TrayApp? trayApp;


    public AppView(
        AppController controller
    )
    {
        this.controller = controller;
        this.cts = new CancellationTokenSource();
    }

    public void Start(RuleSet rules)
    {
        this.trayApp = new TrayApp(this.Dispose);

        //WebSocketなどでもJS側で待ち受けているのでアーキテクチャ上問題ないはず
        this.controller.ObserveRules(
            rules, 
            (rule) => this.StartRule(rule),
            (rule) => this.StopRule(rule),
            cts
        );

        this.trayApp.Run();
    }

    private void Dispose()
    {

    }



    private void StartRule(Rule rule)
    {
        System.Console.WriteLine($"start {rule.Name}");
        
        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            rule.Notions.ForEach((n) => this.ShowNotion(rule, n));
        });
    }

    private void ShowNotion(Rule rule, INotion notion)
    {
        //すでに表示されていたら、何もしない
        if(this.showedNotions.ContainsKey(notion))
        {
            return;
        }

        var args = new ActionArgs(
            ActionDefinition.NULL,
            rule,
            notion,
            Timing.Start
        );


        var ui = notion switch
        {
            Button impl => new NotionButton(impl, () => this.controller.Execute(args with {Action = impl.Action})),
            Tray impl => this.trayApp!.RegisterMenuItem(notion, () => this.controller.Execute(args with {Action = impl.Action})),
            Pipe impl => new DisposableWrapper(
                    () => this.controller.Execute(args with {Action = impl.StartAction}),
                    () => this.controller.Execute(args with {Action = impl.StopAction, Timing = Timing.Stop})
                ),

            _ => throw new Exception("Notionに対応するUIがありません")
        };

        this.showedNotions.Add(notion, ui);
    }

    private void StopRule(Rule rule)
    {
        System.Console.WriteLine($"stop {rule.Name}");
        
        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            rule.Notions.ForEach((n) => this.HideNotion(rule, n));
        });
    }

    private void HideNotion(Rule rule, INotion notion)
    {
        //すでに非表示ならば、何もしない
        if(!this.showedNotions.TryGetValue(notion, out IDisposable? ui))
        {
            return;
        }

        this.showedNotions.Remove(notion);
        ui.Dispose();
    }

}