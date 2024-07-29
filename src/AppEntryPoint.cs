
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using Json.Schema.Generation;

namespace NotifiableTools;

public class AppEntryPoint
{
    // [STAThread]
    // public static async Task<int> Main(string[] args)
    // {
    //     // Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
    //     // Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

    //     App.Call();
    //     return await Task.FromResult(0);
    // }

    // [STAThread]
    // public static void Main(string[] args)
    // {
    //     App.Call();
    // }

    public static async Task<int> Main(string[] args)
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

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