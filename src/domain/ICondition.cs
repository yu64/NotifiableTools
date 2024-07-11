
using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace NotifiableTools;

// [JsonDerivedType(typeof(EqualsCondition), ConditionType.Equals)]
public interface ICondition
{
    public ConditionType Type { get; }
}

public enum ConditionType
{
    Equals
}