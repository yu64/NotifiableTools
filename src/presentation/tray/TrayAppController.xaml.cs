using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using static NotifiableTools.TrayApp;

namespace NotifiableTools;


public partial class TrayAppController : System.Windows.Application, TrayAppHandler
{
    //依存関係の注入

    private readonly RuleSet rules;
    private readonly Usecase usecase;
    private readonly NotionController notionController;

    private CancellationTokenSource? observerCts;

    private TrayApp trayApp;


    public TrayAppController(
        RuleSet rules, 
        Usecase usecase,
        NotionController notionController
    )
    {
        this.rules = rules;
        this.usecase = usecase;
        this.notionController = notionController;

        
        this.trayApp = new TrayApp(this);
    }



    //==============================================================



    public void OnStartupTrayApp()
    {
        this.StartRuleObserver();
    }

    private void StartRuleObserver()
    {
        //ルールの状態監視を開始
        //判定結果を通知関連にリダイレクト
        this.observerCts = this.usecase.ObserveRule(
            this.rules,
            (rule) => this.notionController.ShowNotions(rule, this.trayApp.RegisterMenuItem),
            (rule) => this.notionController.HideNotions(rule)
        );
    }


    public void OnExitTrayApp()
    {
        this.StopRuleObserver();
    }

    private void StopRuleObserver()
    {
        this.observerCts?.Cancel();
    }



    //==============================================================

}

