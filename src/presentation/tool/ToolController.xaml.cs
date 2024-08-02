using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;


namespace NotifiableTools;

public partial class ToolController : Window
{
    private readonly INotion notion;

    

    public ToolController(INotion notion)
    {
        this.notion = notion;

        //UI初期化時に、なぜかDateTime.Now関連でNullReferenceExceptionが投げられる
        //そのため、明示的に使用し、初期化する
        _ = DateTime.Now;

        //UI初期化
        this.InitializeComponent();



        var desktop = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
    }

    
    
}