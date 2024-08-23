
namespace NotifiableTools;

[AllSubType]
public interface IPosition
{

    public System.Windows.Point GetPos(System.Drawing.Point mouse, Rectangle focusedDesktop, double uiWidth, double uiHeight);

}