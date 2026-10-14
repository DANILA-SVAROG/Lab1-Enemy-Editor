using System.Windows;
using ClickerMechanics;
using EnemyEditor.Models;

namespace ClickerGame.Models;

public class CGameController : CController
{
    private readonly Random rng = new Random();

    public CGameController(Player player, Size sceneSize)
        : base(3, 0, sceneSize, player, false)
    {
    }

    // Общий контроллер обновляет все бонусы как CCollectable.
    protected override CCollectable createObject(Point position, double size, double lifetime)
    {
        int type = rng.Next(3);
        if (type == 0) return new CDamageSphere(position, size, lifetime + 3);
        if (type == 1) return new CSpeedSphere(position, size, lifetime + 3);
        return new CGoldSphere(position, size, lifetime + 3);
    }
}
