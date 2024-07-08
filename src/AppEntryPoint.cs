
using System.Diagnostics;

namespace NotifiableTools;

public class AppEntryPoint
{
    [STAThread]
    public static void Main()
    {
        Debug.WriteLine("My Main Method.");
        App app = new();
        app.InitializeComponent();
        app.Run();
    }
}