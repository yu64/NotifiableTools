
namespace NotifiableTools;

public class RuleObserver
{
    private bool _isStop;
    public bool IsStop {
        get
        {
            return this._isStop;
        }
    }

    public RuleObserver()
    {

    }

    public void stop()
    {
        this._isStop = true;
    }


    public async void Observe(RuleSet ruleSet, Action<Rule> tellEnable, Action<Rule> tellDisable)
    {
        //ルールごとに非同期な判定処理を行う
        var tasks = ruleSet.Rules.Select((rule) => Task.Run(async () => {

            //停止指示が検出されるまで無限ループ
            while(!this._isStop)
            {
                //ルールが有効であるか判定
                var isEnable = await rule.Condition.Call();

                //有効であるか、無効であるか伝える
                (isEnable ? tellEnable : tellDisable)(rule);

                //ルールごとの待機時間分、待機する
                Thread.Sleep(rule.IntervalMilliseconds);
            }

            return;
        }));

        await Task.WhenAll(tasks.ToArray());
    }


}