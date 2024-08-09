

namespace NotifiableTools;


public class AppView
{
    private readonly AppController controller;

    private readonly CancellationTokenSource cts;

    private readonly IDictionary<INotion, IDisposable> showedNotions = new Dictionary<INotion, IDisposable>();

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

        //WebSocketなどでもJS側で待ち受けているのでアーキテクチャ上問題ないはず
        this.controller.ObserveRules(
            rules, 
            (rule) => this.StartRule(rule),
            (rule) => this.StopRule(rule),
            cts
        );
        
        this.trayApp = new TrayApp(this.Dispose);
    }

    private void Dispose()
    {

    }



    public void StartRule(Rule rule)
    {
        System.Console.WriteLine($"start {rule.Name}");
        rule.Notions.ForEach((n) => this.ShowNotion(n));
    }

    public void ShowNotion(INotion notion)
    {
        if(!this.showedNotions.ContainsKey(notion))
        {
            return;
        }

        var ui = notion switch
        {
            Button impl => new NotionUi(NotionUi.UiType.Button, (args) => this.controller.Execute(notion, args)),
            Tray impl => this.trayApp.RegisterMenuItem(notion, (args) => this.controller.Execute(notion, args)),
            Pipe impl => new DisposableWrapper(
                    () => this.controller.Execute(impl, new Dictionary<string, string>()),
                    () => this.controller.Execute(impl, new Dictionary<string, string>())
                ),

            _ => throw new Exception("Notionに対応するUIがありません")
        };

        this.showedNotions.Add(notion, ui);
    }

    public void StopRule(Rule rule)
    {
        System.Console.WriteLine($"stop {rule.Name}");
        rule.Notions.ForEach((n) => this.HideNotion(n));
    }

    public void HideNotion(INotion notion)
    {
        if(!this.showedNotions.TryGetValue(notion, out IDisposable? ui))
        {
            return;
        }

        this.showedNotions.Remove(notion);
        ui.Dispose();
    }

}