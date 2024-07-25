
using System.CommandLine.Parsing;

namespace NotifiableTools;

public class ShellExecutor : ICommandExecutor
{
    public void Execute(AbstractAction action)
    {
        System.Console.WriteLine($"実行: {action.Command}");
    }
}