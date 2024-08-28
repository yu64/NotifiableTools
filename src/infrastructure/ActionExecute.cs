

using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using Cysharp.Diagnostics;
using SmartFormat;
using SmartFormat.Extensions;

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
            var smart = new SmartFormatter()
                .AddExtensions(new DefaultFormatter())
                .AddExtensions(
                    new DefaultSource(), 
                    new StringSource(), 
                    new DictionarySource(),
                    new ValueTupleSource(),
                    new ReflectionSource()
                );

            command = SmartFormatUtil.Format(action.CommandTemplate, args);
        }
        catch(Exception ex)
        {
            SmartFormatUtil.PrintErrorMessage(ex, action.CommandTemplate, args);
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