

using System.Diagnostics;
using System.Linq.Expressions;
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

        var enableRules = ruleSet.Rules.Where((r) => r.Enable);

        //ルールごとに非同期な判定処理を行う
        enableRules.ToList().ForEach((rule) => Task.Factory.StartNew(this.WrapRestarter(async () => {
            
            using var ctx = this.contextFuctory(rule);
            
            //無限ループ
            while(true)
            {
                //タスクがキャンセルされたら脱出
                cts.Token.ThrowIfCancellationRequested();

                //ルールの条件を判定
                var isEnable = await rule.Condition.Call(ctx);

                //有効であるか、無効であるか伝える
                (isEnable ? tellEnable : tellDisable)(rule);

                //ルールごとの待機時間分、待機する
                Thread.Sleep(rule.IntervalMilliseconds);
            }


        }), TaskCreationOptions.LongRunning));



        return cts;
    }

    public void Executorule(Rule rule)
    {
        rule.Actions.ForEach((action) => {

            this.commandExecutor.Execute(action);
        });
    }

    private Func<T> WrapRestarter<T>(Func<T> inner)
    {
        return () => {
            
            while (true)
            {
                try
                {
                    return inner();
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    //例外が起きたら再実行
                    ExceptionUtil.Print(e);
                }
            }
            
        };
    }
    
}