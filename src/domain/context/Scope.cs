
namespace NotifiableTools;

public class Scope
{
    public readonly Type OwnerType;

    public readonly object Value;

    public Scope(IAnyFunction owner, object value)
    {
        this.OwnerType = owner.GetType();
        this.Value = value;
    }

}