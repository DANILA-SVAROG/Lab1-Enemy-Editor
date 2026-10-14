using System.Windows;
using System.Windows.Media;
using ClickerMechanics;
using EnemyEditor.Models;

namespace ClickerGame.Models;

public class CGoldSphere : CCollectable
{
    public CGoldSphere(Point position, double size, double lifetime)
        : base(position, size, lifetime)
    {
        sprite.Fill = Brushes.Gold;
        sprite.ToolTip = "Золото: цена следующего улучшения урона";
    }

    public override bool onClick(CPlayer player, CController controller, Point mousePosition)
    {
        if (!isMouseOnObject(mousePosition)) return false;
        Player gamePlayer = (Player)player;
        gamePlayer.AddGold(gamePlayer.UpgradeCost);
        return true;
    }
}
