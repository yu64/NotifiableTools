using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace NotifiableTools;


public partial class TrayApp : System.Windows.Application
{
    private NotifyIcon? trayIcon;
    private ContextMenuStrip menu;

    private TrayAppHandler handler;

    public TrayApp(TrayAppHandler handler)
    {
        this.handler = handler;
        this.InitializeComponent();

        this.Run();
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


        this.handler.OnStartupTrayApp();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        this.handler.OnExitTrayApp();
        this.trayIcon?.Dispose();
    }



    //====================================================================================


    /// <summary>
    /// コンテキストメニューに項目を追加します。戻り値をDisposeすると削除されます。
    /// </summary>
    /// <returns></returns>
    public IDisposable RegisterMenuItem(string? text, Image? image, EventHandler? onClick)
    {
        var item = this.menu.Items.Add(text, image, onClick);

        return new DisposableWrapper(() => this.menu.Items.Remove(item));
    }

    private class DisposableWrapper(Action dispose) : IDisposable
    {
        
        public void Dispose()
        {
            dispose();
        }
    }



    //====================================================================================


    public interface TrayAppHandler 
    {
        public void OnStartupTrayApp();
        public void OnExitTrayApp();
    }


    //====================================================================================


}

