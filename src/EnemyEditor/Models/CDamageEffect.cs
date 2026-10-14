namespace EnemyEditor.Models;

public class CDamageEffect : CBonusEffect
{
    public CDamageEffect(double duration) : base(duration)
    {
    }

    public override double getDamageModifier() => 2;
    public override string getDescription() => "Урон ×2";
}
