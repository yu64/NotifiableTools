using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace NotifiableTools;


public partial class TrayApp : System.Windows.Application
{
    public delegate void OnExitTrayApp();

    private NotifyIcon? trayIcon;
    private ContextMenuStrip menu;

    private OnExitTrayApp handler;

    public TrayApp(OnExitTrayApp handler)
    {
        this.handler = handler;
        this.InitializeComponent();
    }



    //====================================================================================


    /// <summary>
    /// アプリケーション初期化時の処理
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //アプリのアイコン
        var appIcon = AssetEnum.APP_ICON.Create((s) => new Icon(s));

        
        this.menu = new ContextMenuStrip();
        this.menu.Items.Add("終了", null, (_, _) => this.Shutdown());

        //トレイアイコンを作成
        this.trayIcon = new NotifyIcon()
        {
            Visible = true,
            Icon = appIcon,
            Text = "タスクトレイ常駐アプリのテストです",
            ContextMenuStrip = this.menu
        };
        //this.trayIcon.MouseClick += this.OnClickTrayIcon;

    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        this.handler();
        this.trayIcon?.Dispose();
    }



    //====================================================================================



    public IDisposable RegisterMenuItem(INotion notion, Action<IDictionary<string, string>> onSubmit)
    {
        var item = this.menu.Items.Add("項目", null, (sender, args) => onSubmit(new Dictionary<string, string>()));
        return new DisposableWrapper(() => this.menu.Items.Remove(item));
    }




    //====================================================================================


}

