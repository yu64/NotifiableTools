
namespace NotifiableTools;

[AllSubType]
public interface IPosition
{

    public double GetX(Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight);

    public double GetY(Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight);
}