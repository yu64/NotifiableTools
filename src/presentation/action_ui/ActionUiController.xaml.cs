using System.Text;
using System.Windows;


namespace NotifiableTools;

public partial class ActionUiController : Window
{
    private readonly AbstractAction action;

    public ActionUiController(AbstractAction action)
    {
        this.InitializeComponent();
        this.action = action;
    }
}