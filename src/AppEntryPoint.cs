
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
            (rules) => new TrayController(
                rules,
                new Usecase(
                    new ShellExecutor()
                ), 
                (action) => new ActionUiController(action)
            ),
            new RuleParser()
        );

        return await console.Start(args);
    }
}