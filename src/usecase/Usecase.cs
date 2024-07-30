

using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotifiableTools;

public class Usecase
{
    private readonly Func<Rule, IFunctionContext> contextFuctory;
    private readonly ICommandExecutor commandExecutor;

    public Usecase(
        Func<Rule, IFunctionContext> contextFuctory,
        ICommandExecutor commandExecutor
    ) 
    {
        this.contextFuctory = contextFuctory;
        this.commandExecutor = commandExecutor;
    }

    public CancellationTokenSource ObserveRule(RuleSet ruleSet, Action<Rule> tellEnable, Action<Rule> tellDisable)
    {
        var cts = new CancellationTokenSource();

        //ルールごとに非同期な判定処理を行う
        ruleSet.Rules.ForEach((rule) => Task.Factory.StartNew(async () => {
            
            using var ctx = this.contextFuctory(rule);

            //無限ループ
            while(true)
            {
                //タスクがキャンセルされたら脱出
                cts.Token.ThrowIfCancellationRequested();

                //ルールが有効であるか判定
                var isEnable = await rule.Condition.Call(ctx);

                //有効であるか、無効であるか伝える
                (isEnable ? tellEnable : tellDisable)(rule);

                //ルールごとの待機時間分、待機する
                Thread.Sleep(rule.IntervalMilliseconds);
            }


        }, TaskCreationOptions.LongRunning));



        return cts;
    }

    public void Executorule(Rule rule)
    {
        rule.Actions.ForEach((action) => {

            this.commandExecutor.Execute(action);
        });
    }


    
}