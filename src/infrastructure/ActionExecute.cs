

using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;
using Cysharp.Diagnostics;
using SmartFormat;
using SmartFormat.Extensions;

namespace NotifiableTools;

public class ActionExecute : IActionExecutor
{
    
    static ActionExecute()
    {
        //クラス読み込み時に、Encodingの準備をしておく
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public void Execute(ActionArgs args)
    {
        ActionDefinition action = args.Action;
        if(string.IsNullOrWhiteSpace(action.CommandTemplate))
        {
            return;
        }

        var command = this.FormatCommand(args);
        if(string.IsNullOrWhiteSpace(command))
        {
            return;
        }

        this.RunCommand(args, command);
    }
    

    private string FormatCommand(ActionArgs args)
    {
        ActionDefinition action = args.Action;

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

            return SmartFormatUtil.Format(action.CommandTemplate, args);
        }
        catch(Exception ex)
        {
            SmartFormatUtil.PrintErrorMessage(ex, action.CommandTemplate, args);
            return "";
        }
    }


    private void RunCommand(ActionArgs args, string command)
    {
        ActionDefinition action = args.Action;
        try
        {   
            var encoding = Encoding.GetEncoding(action.Encoding);
            var cwd = Directory.GetCurrentDirectory();

            Task.Run(async () => {

                await foreach(var item in ProcessX.StartAsync(command, workingDirectory: cwd, encoding: encoding))
                {
                    if(action.CanStdOut)
                    {
                        System.Console.WriteLine(item);
                    }
                }
            });
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"コマンドの実行に失敗しました。Command:({command}), Template:({action.CommandTemplate}), Args:({args})");
            Console.Error.WriteLine(ex.Message);
        }
    }












}