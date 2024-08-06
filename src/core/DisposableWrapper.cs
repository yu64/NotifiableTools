

namespace NotifiableTools;

public class DisposableWrapper(Action dispose) : IDisposable
{
    
    public void Dispose()
    {
        dispose();
    }
}