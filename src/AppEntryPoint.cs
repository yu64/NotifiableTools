
using System.Diagnostics;
using System.Text.Json;
using Json.Schema.Generation;

namespace NotifiableTools;

public class AppEntryPoint
{
    [STAThread]
    public static async Task<int> Main(string[] args)
    {

        var console = new ConsoleController(
            () => new TrayController(
                new Usecase(), 
                () => new ToolBarController()
            ),
            new RuleParser()
        );

        return await console.Start(args);
    }
}