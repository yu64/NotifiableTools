

using System.ComponentModel.DataAnnotations;

namespace NotifiableTools;

public record class Absolute(

    [property: Required] double x,
    [property: Required] double y

) : IPosition
{
    public System.Windows.Point GetPos(Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight)
    {
        return new System.Windows.Point(
            focusedDesktop.X + x,
            focusedDesktop.Y + y
        );
    }

}
