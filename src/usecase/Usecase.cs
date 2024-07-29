

using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotifiableTools;

public class Usecase
{
    private readonly ICommandExecutor commandExecutor;

    public Usecase(
        ICommandExecutor commandExecutor
    ) 
    {
        this.commandExecutor = commandExecutor;
    }

    public CancellationTokenSource ObserveRule(RuleSet ruleSet, Action<Rule> tellEnable, Action<Rule> tellDisable)
    {
        var cts = new CancellationTokenSource();



        //ルールごとに非同期な判定処理を行う
        ruleSet.Rules.ForEach((rule) => Task.Factory.StartNew(async () => {

            //無限ループ
            while(true)
            {
                //タスクがキャンセルされたら脱出
                cts.Token.ThrowIfCancellationRequested();

                //ルールが有効であるか判定
                var isEnable = await rule.Condition.Call();

                //有効であるか、無効であるか伝える
                (isEnable ? tellEnable : tellDisable)(rule);

                //ルールごとの待機時間分、待機する
                Thread.Sleep(100);
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