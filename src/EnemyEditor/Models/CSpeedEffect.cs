namespace EnemyEditor.Models;

public class CSpeedEffect : CBonusEffect
{
    public CSpeedEffect(double duration) : base(duration)
    {
    }

    public override double getCooldownModifier() => 0.5;
    public override string getDescription() => "Перезарядка ×0,5";
}
