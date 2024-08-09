
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.IO;

namespace NotifiableTools;

public class ConsoleController
{   
    public delegate void AppStarter(RuleSet rules);


    private readonly AppStarter startApp;
    private readonly RuleParser parser;


    public ConsoleController(
        AppStarter startApp,
        RuleParser parser
    )
    {
        this.startApp = startApp;
        this.parser = parser;
    }


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
            
            //ルールを読み込む
            var rules = rulePaths
                .Select((path) => this.parser.ParseFromFile(path))
                .Aggregate((a, b) => a.Merge(b));

            //アプリケーションを起動
            this.startApp(rules);
        });
    }


//=====================================================================================================


}