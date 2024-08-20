

using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotifiableTools;

public class Usecase
{
    public delegate IRuleSetContext RuleSetContextFactory(RuleSet ruleSet);
    public delegate IRuleContext RuleContextFactory(IRuleSetContext ctx, Rule rule);

    private readonly RuleSetContextFactory createRuleSetContext;
    private readonly RuleContextFactory createRuleContext;

    public Usecase(
        RuleSetContextFactory createRuleSetContext,
        RuleContextFactory createRuleContext
    ) 
    {
        this.createRuleSetContext = createRuleSetContext;
        this.createRuleContext = createRuleContext;
    }


    public CancellationTokenSource ObserveRules(
        RuleSet ruleSet, 
        Action<Rule> tellStart, 
        Action<Rule> tellStop,
        CancellationTokenSource? cts = null
    )
    {
        cts ??= new CancellationTokenSource();

        var ruleSetCtx = this.createRuleSetContext(ruleSet);

        //ルールごとに非同期な判定処理を行う
        var tasks = ruleSet.GetEnableRules().Select((rule) => Task.Factory.StartNew(this.WrapRestarter(async () => {
            
            using var ctx = this.createRuleContext(ruleSetCtx, rule);

            //前回の判定結果
            var isMeetPrevCondition = false;
            
            //無限ループ(ループ間隔が負の数である場合、脱出)
            while(true)
            {
                //タスクがキャンセルされたら脱出
                cts.Token.ThrowIfCancellationRequested();

                System.Console.WriteLine($"check {rule.Name}");
                
                //ルールの条件を判定
                var isMeetCondition = await rule.Condition.Call(ctx);

                //判定結果の変化を伝える
                //false => true
                if(!isMeetPrevCondition && isMeetCondition)
                {
                    tellStart(rule);
                }

                //true => false
                if(isMeetPrevCondition && !isMeetCondition)
                {
                    tellStop(rule);
                }

                //前回の判定結果を保存
                isMeetPrevCondition = isMeetCondition;

                //ルールごとの待機時間が負数である場合、ループ終了
                if(rule.IntervalMilliseconds < 0)
                {
                    break;
                }

                //ルールごとの待機時間分、待機する
                Thread.Sleep(rule.IntervalMilliseconds);
            }

        }), TaskCreationOptions.LongRunning));

        //全てのタスクを非同期で待機する
        Task.WhenAll(tasks).ContinueWith(task => {

            ruleSetCtx.Dispose();
        });


        return cts;
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