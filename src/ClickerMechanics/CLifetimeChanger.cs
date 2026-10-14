using System.Windows;
using System.Windows.Media;

namespace ClickerMechanics;

public class CLifetimeChanger : CCollectable
{
    private double lifetimeModifier;

    public CLifetimeChanger(Point position, double size, double lifetime, double lifetimeModifier)
        : base(position, size, lifetime)
    {
        this.lifetimeModifier = lifetimeModifier;
        sprite.Fill = Brushes.LimeGreen;
        sprite.ToolTip = "Увеличение времени жизни сфер";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (isMouseOnObject(mousePosition) == false)
            return false;

        controller.increaseLifetime(lifetimeModifier);
        return true;
    }
}
