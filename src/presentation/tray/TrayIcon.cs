using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace NotifiableTools;


public partial class TrayIcon : System.Windows.Application
{
    private TrayController callbackController;
    private NotifyIcon? trayIcon;


    public TrayIcon(
        TrayController callbackController
    )
    {
        this.callbackController = callbackController;
        this.InitializeComponent();
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


        this.callbackController.OnStartup();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        this.callbackController.OnExit();
        this.trayIcon.Dispose();
    }


    
}

