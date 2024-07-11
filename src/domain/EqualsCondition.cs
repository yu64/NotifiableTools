
namespace NotifiableTools;

public class EqualsCondition : ICondition
{
    public ConditionType Type => ConditionType.Equals;

    public string Left { get; }
    public string Right { get; }
}