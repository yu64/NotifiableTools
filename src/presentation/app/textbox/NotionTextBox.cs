using System;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Input;


namespace NotifiableTools;

public partial class NotionTextBox : Window, IDisposable
{
    private readonly CancellationTokenSource cts = new CancellationTokenSource();

    public NotionTextBox(TextBox notion, Action<ActionDefinition, string> onSummit)
    {
        //UI初期化時に、なぜかDateTime.Now関連でNullReferenceExceptionが投げられる
        //そのため、明示的に使用し、初期化する
        _ = DateTime.Now;

        //UI初期化
        this.InitializeComponent();

        //コントロール初期設定
        this.ContentTextBox.Visibility = Visibility.Visible;
        this.ContentTextBox.Text = notion.DefaultText;
        this.ContentTextBox.PreviewKeyDown += (sender, args) => {
            
            if( !(args.Key == Key.Enter) )
            {
                return;
            }
            
            onSummit(notion.Action, this.ContentTextBox.Text);
            this.Hide();
        };

        this.setWindowLocation(notion.Position);



        this.Show();
    }

    private void setWindowLocation(IPosition position)
    {
        this.WindowStartupLocation = WindowStartupLocation.Manual;

        var mouse = System.Windows.Forms.Cursor.Position;
        var desktop = Screen.PrimaryScreen!.WorkingArea;
        

        this.ContentTextBox.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
        var size = this.ContentTextBox.DesiredSize;

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