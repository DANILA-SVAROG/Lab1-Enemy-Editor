namespace ClickerMechanics;

public class CCountdownTimer
{
    private double targetTime;

    public double getTime()
    {
        return targetTime;
    }

    public void start(double duration)
    {
        targetTime = Math.Max(0, duration);
    }

    public void update(double delta)
    {
        targetTime = Math.Max(0, targetTime - Math.Max(0, delta));
    }
}
