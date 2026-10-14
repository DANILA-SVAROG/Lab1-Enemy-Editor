using System.Windows;
using System.Windows.Media;
using ClickerMechanics;
using EnemyEditor.Models;

namespace ClickerGame.Models;

public class CDamageSphere : CCollectable
{
    public CDamageSphere(Point position, double size, double lifetime)
        : base(position, size, lifetime)
    {
        sprite.Fill = Brushes.IndianRed;
        sprite.ToolTip = "Урон ×2 на 10 секунд";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (!isMouseOnObject(mousePosition)) return false;
        ((Player)player).AddBonus(new CDamageEffect(10));
        return true;
    }
}
