using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace NotifiableTools;


public partial class TrayController : System.Windows.Application
{
    //依存関係の注入

    private readonly RuleSet rules;
    private readonly Usecase usecase;
    private readonly Func<AbstractAction, ActionUiController> actionUiFactory;


    private CancellationTokenSource observerCts;
    private Dictionary<Rule, ImmutableList<Window>> ruleToUiChildren = [];
    private NotifyIcon trayIcon;




    public TrayController(
        RuleSet rules, 
        Usecase usecase,
        Func<AbstractAction, ActionUiController> actionUiFactory
    )
    {
        this.rules = rules;
        this.usecase = usecase;
        this.actionUiFactory = actionUiFactory;

        //this.InitializeComponent();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //アプリのアイコン
        var appIcon = AssetEnum.APP_ICON.Create((s) => new Icon(s));

        
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("終了", null, (_, _) => this.Shutdown());

        //トレイアイコンを作成
        this.trayIcon = new NotifyIcon()
        {
            Visible = true,
            Icon = appIcon,
            Text = "タスクトレイ常駐アプリのテストです",
            ContextMenuStrip = contextMenu
        };
        //this.trayIcon.MouseClick += this.OnClickTrayIcon;


        //ルールの状態監視を開始
        this.observerCts = this.usecase.ObserveRule(
            this.rules,
            this.EnableActionUi,
            this.DisableActionUi
        );

    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        this.trayIcon.Dispose();
        this.observerCts.Cancel();
    }


    private void EnableActionUi(Rule rule)
    {
        System.Console.WriteLine($"enable {rule.Name}");
        
        if(this.ruleToUiChildren.ContainsKey(rule))
        {
            return;
        }
        
        //(UI)メインスレッドで同期実行する
        this.Dispatcher.Invoke(() => {

            var children = rule.Actions.Select((v) => (Window)this.actionUiFactory(v)).ToImmutableList();
        
            this.ruleToUiChildren.Add(rule, children);

            foreach(var child in children)
            {
                child.Show();
            }
        });

        
    }
    private void DisableActionUi(Rule rule)
    {
        System.Console.WriteLine($"disable {rule.Name}");
        
        if(!ruleToUiChildren.TryGetValue(rule, out ImmutableList<Window>? chlidren))
        {
            return;
        }

        this.ruleToUiChildren.Remove(rule);

        //(UI)メインスレッドで同期実行する
        this.Dispatcher.Invoke(() => {

            chlidren.ForEach((w) => {
            
                w.Close();
            });
        });
        
        
    }

    private void OnClickTrayIcon(object? sender, MouseEventArgs e)
    {
        
    }
}

