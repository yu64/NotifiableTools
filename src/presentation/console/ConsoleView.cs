
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.IO;

namespace NotifiableTools;

public class ConsoleView
{   
    private ConsoleController controller;
    private readonly RootCommand root;


    public ConsoleView(
        ConsoleController controller
    )
    {
        this.controller = controller;
        this.root = this.DefineCommand();
    }

    public async Task<int> Start(string[] args)
    {
        return await this.root.InvokeAsync(args);
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