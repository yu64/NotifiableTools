using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.EventHandlers;
using FlaUI.UIA3;
using Json.Schema.Generation;
using System.Threading.Channels;

namespace NotifiableTools;

[method: JsonConstructor]
public readonly record struct DetectFocusedElement() : IUiElementFunction
{

    public async Task<AutomationElement?> Call(IRuleContext ctx)
    {
        var buf = ctx.ruleSetContext.RunExclusively(
            () => new UIA3Automation(),
            (auto) => ctx.ruleSetContext.GetOrCreateDisposable(() => new FocusedElementQueue(auto))
        );

        var result = await buf.Enqueue();

        return result;
    }


    private class FocusedElementQueue : IDisposable
    {   
        
        private FocusChangedEventHandlerBase handlerId;

        private Channel<AutomationElement> queue = Channel.CreateUnbounded<AutomationElement>();

        public FocusedElementQueue(AutomationBase auto)
        {
            
            this.handlerId = auto.RegisterFocusChangedEvent(async (ele) => {
            
  
                await queue.Writer.WriteAsync(ele);
            });
        }

        public async Task<AutomationElement> Enqueue()
        {
            var ele = await queue.Reader.ReadAsync();
            return ele;
        }

        public void Dispose()
        {
            this.handlerId.Dispose();
        }
    }

}
