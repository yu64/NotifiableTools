

namespace NotifiableTools;

public record class TopLeft : IPosition
{
    public System.Windows.Point GetPos(Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight)
    {
        return new System.Windows.Point(
            focusedDesktop.X,
            focusedDesktop.Y
        );
    }
}
