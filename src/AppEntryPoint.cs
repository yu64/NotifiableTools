
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using Json.Schema.Generation;

namespace NotifiableTools;

public class AppEntryPoint
{

    public static async Task<int> Main(string[] args)
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);


        var usecase = new Usecase(
            (ruleSet) => new RuleSetContext(ruleSet),
            (ctx, rule) => new RuleContext(ctx, rule)
        );

        var appController = new AppController(usecase);
        var app = new AppView(appController);

        var ruleParser = new RuleParser();
        var consoleControl = new ConsoleController(app.Start, ruleParser);
        var console = new ConsoleView(consoleControl);

        return await console.Start(args);
    }
}