using System.Windows;
using System.Windows.Media;
using ClickerMechanics;
using EnemyEditor.Models;

namespace ClickerGame.Models;

public class CSpeedSphere : CCollectable
{
    public CSpeedSphere(Point position, double size, double lifetime)
        : base(position, size, lifetime)
    {
        sprite.Fill = Brushes.DodgerBlue;
        sprite.ToolTip = "Перезарядка ×0,5 на 8 секунд";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (!isMouseOnObject(mousePosition)) return false;
        ((Player)player).AddBonus(new CSpeedEffect(8));
        return true;
    }
}
