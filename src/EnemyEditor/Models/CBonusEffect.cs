namespace EnemyEditor.Models;

public abstract class CBonusEffect
{
    private double timeLeft;

    protected CBonusEffect(double duration)
    {
        timeLeft = duration;
    }

    public double getTime() => timeLeft;
    public virtual double getDamageModifier() => 1;
    public virtual double getCooldownModifier() => 1;
    public abstract string getDescription();

    public bool update(double delta)
    {
        timeLeft = Math.Max(0, timeLeft - Math.Max(0, delta));
        return timeLeft <= 0;
    }
}
