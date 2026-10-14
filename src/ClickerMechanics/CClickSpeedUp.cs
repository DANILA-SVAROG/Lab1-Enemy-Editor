using System.Windows;
using System.Windows.Media;

namespace ClickerMechanics;

public class CClickSpeedUp : CCollectable
{
    private double speedModifier;

    public CClickSpeedUp(Point position, double size, double lifetime, double speedModifier)
        : base(position, size, lifetime)
    {
        this.speedModifier = speedModifier;
        sprite.Fill = Brushes.DodgerBlue;
        sprite.ToolTip = "Уменьшение перезарядки клика";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (isMouseOnObject(mousePosition) == false)
            return false;

        player.increaseSpeed(speedModifier);
        return true;
    }
}
