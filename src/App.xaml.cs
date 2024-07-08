using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace NotifiableTools;


/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

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

