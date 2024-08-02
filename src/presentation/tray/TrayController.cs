using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace NotifiableTools;


public class TrayController
{
    //依存関係の注入

    private readonly RuleSet rules;
    private readonly Usecase usecase;
    private readonly Func<INotion, NotionController> notionFactory;


    private CancellationTokenSource? observerCts;
    private Dictionary<Rule, ImmutableList<NotionController>> ruleToNotions = [];

    private TrayIcon trayIcon;


    public TrayController(
        RuleSet rules, 
        Usecase usecase,
        Func<INotion, NotionController> notionFactory
    )
    {
        this.rules = rules;
        this.usecase = usecase;
        this.notionFactory = notionFactory;

        this.trayIcon = new TrayIcon(this);
        this.trayIcon.Run();


    }

    public void OnStartup()
    {
        //ルールの状態監視を開始
        this.observerCts = this.usecase.ObserveRule(
            this.rules,
            (rule) => this.StartNotions(rule),
            (rule) => this.StopNotions(rule)
        );
    }



    public void OnExit()
    {
        this.observerCts.Cancel();
    }


    private void StartNotions(Rule rule)
    {
        System.Console.WriteLine($"enable {rule.Name}");
        
        if(this.ruleToNotions.ContainsKey(rule))
        {
            return;
        }
        
        //(UI)メインスレッドで同期実行する
        this.trayIcon.Dispatcher.Invoke(() => {

            var children = rule.Notions.Select((v) => this.notionFactory(v)).ToImmutableList();
            this.ruleToNotions.Add(rule, children);
        });

        
    }
    private void StopNotions(Rule rule)
    {
        System.Console.WriteLine($"disable {rule.Name}");
        
        if(!ruleToNotions.TryGetValue(rule, out ImmutableList<NotionController>? chlidren))
        {
            return;
        }

        this.ruleToNotions.Remove(rule);

        //(UI)メインスレッドで同期実行する
        this.trayIcon.Dispatcher.Invoke(() => {

            chlidren.ForEach((w) => {
            
                w.Close();
            });
        });
        
        
    }

}

