using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;


namespace NotifiableTools;

public partial class ActionUiController : Window
{
    private readonly AbstractAction action;

    

    public ActionUiController(AbstractAction action)
    {
        //UI初期化時に、なぜかDateTime.Now関連でNullReferenceExceptionが投げられる
        //そのため、明示的に使用し、初期化する
        _ = DateTime.Now;

        //UI初期化
        this.InitializeComponent();

        this.action = action;

        this.Title = action.Name ?? this.Title;

        var desktop = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
    }

    
    
}