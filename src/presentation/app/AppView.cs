

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
            (ctx, rule) => this.StartRule(ctx, rule),
            (ctx, rule) => this.StopRule(ctx, rule),
            cts
        );

        this.trayApp.Run();
    }

    private void Dispose()
    {

    }



    private void StartRule(IRuleContext ctx, Rule rule)
    {
        System.Console.WriteLine($"start {rule.Name}");
        
        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            rule.Notions.ForEach((n) => this.ShowNotion(ctx, rule, n));
        });
    }

    private void ShowNotion(IRuleContext ctx, Rule rule, INotion notion)
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
            Timing.Start,
            "",
            ctx.customArgs
        );


        // 読みにくい
        var ui = notion switch
        {
            Button impl => new NotionButton(impl, (action) => this.controller.Execute(args with {Action = action})),
            TextBox impl => new NotionTextBox(impl, (action, text) => this.controller.Execute(args with {Action = action, InputText = text})),
            Tray impl => this.trayApp!.RegisterMenuItem(impl, (action) => this.controller.Execute(args with {Action = action})),
            Pipe impl => new DisposableWrapper(
                    () => this.controller.Execute(args with {Action = impl.StartAction}),
                    () => this.controller.Execute(args with {Action = impl.StopAction, Timing = Timing.Stop})
                ),

            _ => throw new Exception("Notionに対応するUIがありません")
        };

        this.showedNotions.Add(notion, ui);
    }

    private void StopRule(IRuleContext ctx, Rule rule)
    {
        System.Console.WriteLine($"stop {rule.Name}");
        
        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            rule.Notions.ForEach((n) => this.HideNotion(ctx, rule, n));
        });
    }

    private void HideNotion(IRuleContext ctx, Rule rule, INotion notion)
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