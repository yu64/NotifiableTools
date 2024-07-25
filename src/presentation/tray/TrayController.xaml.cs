using System.Windows;
using System.Windows.Media;

namespace NotifiableTools;


public partial class TrayController : System.Windows.Application
{
    //依存関係の注入

    private readonly RuleSet rules;
    private readonly Usecase usecase;
    private readonly Func<Action, ActionUiController> actionUiFactory;


    private CancellationTokenSource observerCts;
    private NotifyIcon trayIcon;




    public TrayController(
        RuleSet rules, 
        Usecase usecase,
        Func<Action, ActionUiController> actionUiFactory
    )
    {
        this.rules = rules;
        this.usecase = usecase;
        this.actionUiFactory = actionUiFactory;
        this.InitializeComponent();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //アプリのアイコン
        var appIcon = AssetEnum.APP_ICON.Create((s) => new Icon(s));

        
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("終了", null, (_, _) => this.StopApp());

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

    private void StopApp()
    {
        this.Shutdown();
        this.observerCts.Cancel();
    }

    private void EnableActionUi(Rule rule)
    {
        System.Console.WriteLine($"enable {rule.Name}");
    }
    private void DisableActionUi(Rule rule)
    {
        System.Console.WriteLine($"disable {rule.Name}");
    }

    private void OnClickTrayIcon(object? sender, MouseEventArgs e)
    {
        
    }
}

