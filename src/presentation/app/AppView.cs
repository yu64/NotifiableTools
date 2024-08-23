

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
            rule.Notions.ForEach((n) => this.ShowNotion(n));
        });
    }

    private void ShowNotion(INotion notion)
    {
        //すでに表示されていたら、何もしない
        if(this.showedNotions.ContainsKey(notion))
        {
            return;
        }

        var ui = notion switch
        {
            Button impl => new NotionButton(impl, (args) => this.controller.Execute(notion, args)),
            Tray impl => this.trayApp!.RegisterMenuItem(notion, (args) => this.controller.Execute(notion, args)),
            Pipe impl => new DisposableWrapper(
                    () => this.controller.Execute(impl, new Dictionary<string, string>()),
                    () => this.controller.Execute(impl, new Dictionary<string, string>())
                ),

            _ => throw new Exception("Notionに対応するUIがありません")
        };

        this.showedNotions.Add(notion, ui);
    }

    private void StopRule(Rule rule)
    {
        System.Console.WriteLine($"stop {rule.Name}");
        
        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            rule.Notions.ForEach((n) => this.HideNotion(n));
        });
    }

    private void HideNotion(INotion notion)
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