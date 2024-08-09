

namespace NotifiableTools;

public class DisposableWrapper(Action dispose) : IDisposable
{

    public DisposableWrapper(Action starter, Action dispose) : this(dispose)
    {
        starter();
    }
    
    public void Dispose()
    {
        dispose();
    }
}