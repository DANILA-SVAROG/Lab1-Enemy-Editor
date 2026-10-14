using System.Windows;
using System.Windows.Media;

namespace ClickerMechanics;

public class CPointGiver : CCollectable
{
    private double pointsValue;

    public CPointGiver(Point position, double size, double lifetime)
        : base(position, size, lifetime)
    {
        sprite.Fill = Brushes.BlueViolet;
        pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        sprite.ToolTip = $"Очки: +{pointsValue:F1}";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (isMouseOnObject(mousePosition) == false)
            return false;

        controller.pointsIncrease(pointsValue);
        return true;
    }
}
