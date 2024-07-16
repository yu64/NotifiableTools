

using System.Runtime.Serialization;
using System.Windows;
using Json.Schema.Generation;

namespace NotifiableTools;


[SubType(typeof(EqualsCondition), typeof(SampleCondition))]
public abstract class Condition
{
    [Required]
    public abstract ConditionType Type { get; }

}

public enum ConditionType
{
    Equals = 0,
    Sample = 1
}

public class EqualsCondition : Condition
{

    [Required]
    [ConstEnumName((int)ConditionType.Equals)]
    //[Const("Equals_Enum")]
    public override ConditionType Type => ConditionType.Equals;

    [Required]
    [Const("Equals_Text")]
    public string sample;

    [Required]
    public string Left { get; }
    
    [Required]
    public string Right { get; }

}

public class SampleCondition : Condition
{
    [Required]
    [ConstEnumName((int)ConditionType.Sample)]
    //[Const("Sample_Enum")]
    public override ConditionType Type => ConditionType.Sample;

    [Required]
    [Const("Sample_Text")]
    public string sample;

    [Required]
    public string A { get; }

    [Required]
    public string B { get; }

}