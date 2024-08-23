

using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;

public record class MouseCircle : IPosition
{   
    [Required]
    [Minimum(0)]
    [Maximum(359)]
    public float Angle { get; }

    [Required]
    [Minimum(0)]
    public float Radius { get; }


    [JsonConstructor]
    public MouseCircle(float angle, float radius)
    {
        //0度は右を意味するので、上が0度になるように変換する。
        this.Angle = (angle + 270) % 360;
        this.Radius = radius;
    }

    public System.Windows.Point GetPos(Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight)
    {
        var radian = this.Angle * Math.PI/ 180f;

        return new System.Windows.Point(
            mouse.X + Math.Cos(radian) * this.Radius - (uiWidth / 2),
            mouse.Y + Math.Sin(radian) * this.Radius - (uiHeight / 2)
        );
    }
}