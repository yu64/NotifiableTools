using System;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Input;


namespace NotifiableTools;

public partial class NotionButton : Window, IDisposable
{
    private readonly CancellationTokenSource cts = new CancellationTokenSource();

    public NotionButton(Button notion, Action<ActionDefinition> onSummit)
    {
        //UI初期化時に、なぜかDateTime.Now関連でNullReferenceExceptionが投げられる
        //そのため、明示的に使用し、初期化する
        _ = DateTime.Now;

        //UI初期化
        this.InitializeComponent();

        //コントロール初期設定
        this.ContentButton.Visibility = Visibility.Visible;
        this.ContentButton.Content = notion.Title;
        this.Focusable = false;
        this.ContentButton.PreviewMouseDown += ((sender, args) => {
            
            if( !(args.LeftButton == MouseButtonState.Pressed) )
            {
                return;
            }

            onSummit(notion.Action);
        });

        this.setWindowLocation(notion.Position);



        this.Show();
    }

    private void setWindowLocation(IPosition position)
    {
        this.WindowStartupLocation = WindowStartupLocation.Manual;

        var mouse = System.Windows.Forms.Cursor.Position;
        var desktop = Screen.PrimaryScreen!.WorkingArea;
        

        this.ContentButton.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
        var size = this.ContentButton.DesiredSize;

        var pos = position.GetPos(mouse, desktop, size.Width, size.Height);
        this.Left = pos.X;
        this.Top = pos.Y;
    }

    public void Dispose()
    {
        this.cts.Cancel();
        this.Dispatcher.Invoke(() => this.Close());
    }

    
}