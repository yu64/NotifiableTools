

using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using Cysharp.Diagnostics;
using SmartFormat;

namespace NotifiableTools;

public class ActionExecute : IActionExecutor
{
    public void Execute(ActionArgs args)
    {
        ActionDefinition action = args.Action;
        if(string.IsNullOrWhiteSpace(action.CommandTemplate))
        {
            return;
        }

        string command;
        try
        {
            command = Smart.Format(action.CommandTemplate, args);
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"コマンドテンプレートの解釈に失敗しました。Template:{action.CommandTemplate}, Args:{args}");
            Console.Error.WriteLine(ex.Message);
            return;
        }

        try
        {
            ProcessX.StartAsync(command);
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"コマンドの実行に失敗しました。Command:({command}), Template:({action.CommandTemplate}), Args:({args})");
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}