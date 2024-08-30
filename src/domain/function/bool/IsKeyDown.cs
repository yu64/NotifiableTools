using System.Text.Json.Serialization;
using System.Windows.Input;
using Json.Schema.Generation;

namespace NotifiableTools;


public readonly record struct IsKeyDown (

    [property: Required] 
    [property: MinItems(1)]
    [property: MaxItems(3)]
    Key[] Keys


) : IBoolFunction
{

    
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern ushort GetKeyState(int nVirtKey);

    private const ushort IS_KEY_DOWN = 0b_1000_0000_0000_0000;

    public Task<bool> Call(IRuleContext ctx)
    {
        bool flag = true;

        foreach(var key in this.Keys)
        {
            var virtualKey = (Keys)KeyInterop.VirtualKeyFromKey(key);
            flag = flag && ((IsKeyDown.GetKeyState((int)virtualKey) & IS_KEY_DOWN) == IS_KEY_DOWN);
        }

        return Task.FromResult(flag);
    }
}
