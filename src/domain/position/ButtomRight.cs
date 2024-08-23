

namespace NotifiableTools;

public record class ButtomRight : IPosition
{
    public System.Windows.Point GetPos(Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight)
    {
        return new System.Windows.Point(
            focusedDesktop.X + focusedDesktop.Width - uiWidth,
            focusedDesktop.Y + focusedDesktop.Height - uiHeight
        );
    }

}
