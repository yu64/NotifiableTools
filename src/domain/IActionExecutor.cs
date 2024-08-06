


namespace NotifiableTools;

public interface IActionExecutor
{

    public void Execute(INotion notion, IDictionary<String, String> args);

}