using System.Windows;
using System.Windows.Media;

namespace ClickerMechanics;

public class CSpawnRateChanger : CCollectable
{
    private double speedModifier;

    public CSpawnRateChanger(Point position, double size, double lifetime, double speedModifier)
        : base(position, size, lifetime)
    {
        this.speedModifier = speedModifier;
        sprite.Fill = Brushes.Orange;
        sprite.ToolTip = "Ускорение появления новых сфер";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (isMouseOnObject(mousePosition) == false)
            return false;

        controller.increaseSpawnSpeed(speedModifier);
        return true;
    }
}
