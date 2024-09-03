
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;

namespace NotifiableTools;

public class ConsoleView
{   
    private ConsoleController controller;

    private readonly RootCommand root;
    private readonly Parser parser;


    public ConsoleView(
        ConsoleController controller
    )
    {

        this.controller = controller;

        this.root = this.DefineCommand();
        this.parser = new CommandLineBuilder(this.root)
            .UseDefaults()
            .Build();
    }

    public async Task<int> Start(string[] args)
    {
        //事前確認
        var result = this.parser.Parse(args);
        var hasError = (0 < result.Errors.Count);

        if(!hasError)
        {
            //実際に実行
            return await this.parser.InvokeAsync(args);
        }

        this.OpenConsole();

        //既定のエラーメッセージを表示
        foreach (var error in result.Errors)
        {
            Console.WriteLine(error.Message);
        }

        //ヘルプ
        var helpBuilder = new HelpBuilder(LocalizationResources.Instance);
        helpBuilder.Write(root, Console.Out);
        return 1;
    }


//=====================================================================================================

    [DllImport("Kernel32.dll")]
    protected static extern Boolean AttachConsole(Int32 processId);
    
    [DllImport("Kernel32.dll")]
    protected static extern Boolean AllocConsole();
    
    [DllImport("Kernel32.dll")]
    protected static extern Boolean FreeConsole();

    public void OpenConsole()
    {
        if(AttachConsole(-1))
        {
            return;
        }

        if(AllocConsole())
        {
            return;
        }
    }



//=====================================================================================================


    /// <summary>
    /// コマンド定義
    /// </summary>
    private RootCommand DefineCommand()
    {
        //引数
        Argument<string[]> rulePaths = new Argument<string[]>(
            "rulePaths",
            "ルールファイルの配列"
        );

        //オプション
        Option<string> output = new Option<string>(
            aliases: ["--output", "-o"], 
            description: "出力先フォルダ",
            getDefaultValue: () => Directory.GetCurrentDirectory()
        );


        //コマンド体系を定義
        return new RootCommand()
        {
            new SubCommand("template", "設定ファイルのテンプレートを生成します")
            {
                output,

                CommandHandler.Create(this.controller.CreateTemplate)
                
            },

            new SubCommand("start", "アプリケーションを起動します")
            {
                rulePaths,

                CommandHandler.Create(this.controller.StartApp)
            }
        };
    }



//=====================================================================================================


    /// <summary>
    /// ハンドラーを追加する Addメソッドを追加したもの
    /// </summary>
    private class SubCommand : Command
    {
        public SubCommand(string name, string? description = null) : base(name, description)
        {
            
        }

        public void Add(ICommandHandler handler)
        {
            this.Handler = handler;
        }
    }



//=====================================================================================================


}