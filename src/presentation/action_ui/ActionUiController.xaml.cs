using System.Text;
using System.Windows;


namespace NotifiableTools;

public partial class ActionUiController : Window
{
    private readonly Action action;

    public ActionUiController(Action action)
    {
        this.InitializeComponent();
        this.action = action;
    }
}