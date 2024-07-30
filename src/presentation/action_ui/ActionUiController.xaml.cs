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
        this.InitializeComponent();
        this.action = action;

        this.Title = action.Name ?? this.Title;
    }

    
}