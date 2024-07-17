
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.IO;

namespace NotifiableTools;

public class ConsoleController
{   
    private readonly Func<TrayController> trayFactory;
    private readonly RuleParser parser;

    private readonly RootCommand root;



    public ConsoleController(
        Func<TrayController> trayFactory,
        RuleParser parser
    )
    {
        this.trayFactory = trayFactory;
        this.parser = parser;
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
            aliases: new string[] {"--output", "-o"}, 
            description: "出力先フォルダ",
            getDefaultValue: () => Directory.GetCurrentDirectory()
        );
  

        //コマンド体系を定義
        return new()
        {
            new SubCommand("template", "設定ファイルのテンプレートを生成します")
            {
                output,

                CommandHandler.Create(this.CreateTemplate)
            },

            new SubCommand("start", "アプリケーションを起動します")
            {
                rulePaths,

                CommandHandler.Create(this.StartApp)
            }
        };
    }



//=====================================================================================================


    public int CreateTemplate(string output)
    {
        return ExceptionUtil.TryCatch(0, 1, () => {

            var schema = this.parser.GenerateJsonSchema();
            var sample = this.parser.GenerateYamlSample("./schema.json");

            Directory.CreateDirectory(output);

            var schemaPath = Path.Combine(output, "schema.json");
            var samplePath = Path.Combine(output, "rule_sample.yaml");

            File.WriteAllText(schemaPath, schema);
            File.WriteAllText(samplePath, sample);
        });
    }


    public int StartApp(string[] rulePaths)
    {
        return ExceptionUtil.TryCatch(0, 1, () => {

            var rules = rulePaths
                .Select((path) => this.parser.ParseFromFile(path))
                .Aggregate((a, b) => a.Merge(b));

            System.Console.WriteLine(rules);
            
            //タスクトレイにアイコンを追加
            //this.trayFactory().Run();
        });

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