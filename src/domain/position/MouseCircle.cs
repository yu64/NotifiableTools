

using System.Text.Json.Serialization;

namespace NotifiableTools;

public record class MouseCircle : IPosition
{
    public float Degree { get; }
    public float Radius { get; }


    [JsonConstructor]
    public MouseCircle(float degree, float radius)
    {
        this.Degree = degree;
        this.Radius = radius;
    }
}