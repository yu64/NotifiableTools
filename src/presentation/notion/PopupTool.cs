using System;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;


namespace NotifiableTools;

public partial class PopupTool : Window, IDisposable
{
    

    public PopupTool(PopupType type)
    {

        //UI初期化時に、なぜかDateTime.Now関連でNullReferenceExceptionが投げられる
        //そのため、明示的に使用し、初期化する
        _ = DateTime.Now;

        //UI初期化
        this.InitializeComponent();
        
        this.Show();

        var desktop = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
        
    }

    public void Dispose()
    {
        this.Dispatcher.Invoke(() => this.Close());
    }

    public enum PopupType
    {
        Button
    }
    
}