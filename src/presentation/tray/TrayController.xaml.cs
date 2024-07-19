using System.Windows;

namespace NotifiableTools;


public partial class TrayController : System.Windows.Application
{
    private readonly RuleSet rules;
    private readonly Usecase usecase;
    private readonly Func<ToolBarController> toolbarFactory;


    public TrayController(
        RuleSet rules, 
        Usecase usecase,
        Func<ToolBarController> toolbarFactory
    )
    {
        this.rules = rules;
        this.usecase = usecase;
        this.toolbarFactory = toolbarFactory;
        this.InitializeComponent();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        e.Args.ToList().ForEach(Console.WriteLine);

        //アプリのアイコン
        var appIcon = AssetEnum.APP_ICON.Create((s) => new Icon(s));

        
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("終了", null, (_, _) => this.Shutdown());


        var notifyIcon = new NotifyIcon()
        {
            Visible = true,
            Icon = appIcon,
            Text = "タスクトレイ常駐アプリのテストです",
            ContextMenuStrip = contextMenu
        };
        //notifyIcon.MouseClick += new MouseEventHandler();

        
    }
}

