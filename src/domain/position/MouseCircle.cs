

using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class MouseCircle : IPosition
{   
    [Required]
    [Minimum(0)]
    [Maximum(359)]
    public float Degree { get; }

    [Required]
    [Minimum(0)]
    public float Radius { get; }


    [JsonConstructor]
    public MouseCircle(float degree, float radius)
    {
        this.Degree = degree;
        this.Radius = radius;
    }
}