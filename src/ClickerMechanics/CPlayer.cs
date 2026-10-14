namespace ClickerMechanics;

public class CPlayer
{
    private double timeBeforeClick;
    private readonly CCountdownTimer countdownTimer;

    public CPlayer(double timeBeforeClick)
    {
        this.timeBeforeClick = timeBeforeClick;
        countdownTimer = new CCountdownTimer();
    }

    public bool canClick() => countdownTimer.getTime() <= 0;
    public double getTimeBeforeClick() => timeBeforeClick;
    public double getRemainingCooldown() => countdownTimer.getTime();

    // Запускает перезарядку только после успешного клика.
    public bool tryClick()
    {
        if (!canClick()) return false;
        countdownTimer.start(getTimeBeforeClick());
        return true;
    }

    public void update(double delta)
    {
        countdownTimer.update(delta);
    }

    public void increaseSpeed(double speedModifier)
    {
        if (speedModifier <= 0 || !double.IsFinite(speedModifier))
        {
            throw new ArgumentOutOfRangeException(nameof(speedModifier));
        }

        timeBeforeClick = Math.Max(0.08, timeBeforeClick * speedModifier);
    }
}
