using System;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Forms;
using System.ComponentModel;


namespace NotifiableTools;

public partial class NotionUi : Window, IDisposable
{
    private readonly Action<IDictionary<string, string>> onSummit;

    public NotionUi(Action<IDictionary<string, string>> onSummit)
    {
        //UI初期化時に、なぜかDateTime.Now関連でNullReferenceExceptionが投げられる
        //そのため、明示的に使用し、初期化する
        _ = DateTime.Now;

        //UI初期化
        this.InitializeComponent();
        this.onSummit = onSummit;
    }

    public NotionUi(Button notion, Action<IDictionary<string, string>> onSummit) : this(onSummit)
    {

        this.SetupPosition(this.OnlyButton, notion.position);
        
        this.Show();
    }

    private void SetupPosition(FrameworkElement  c, IPosition position)
    {
        c.Visibility = Visibility.Visible;
        
        this.WindowStartupLocation = WindowStartupLocation.Manual;

        var mouse = System.Windows.Forms.Cursor.Position;
        var desktop = Screen.PrimaryScreen.Bounds;

        this.Left = position.GetX(mouse, desktop, c.Width, c.Height);
        this.Top = position.GetY(mouse, desktop, c.Width, c.Height);

        // this.Left = System.Windows.Forms.Cursor.Position.X + position.XBasedOnMouse;
        // this.Top = System.Windows.Forms.Cursor.Position.Y + position.YBasedOnMouse;
    }


    public void Dispose()
    {
        this.Dispatcher.Invoke(() => this.Close());
    }

    
}