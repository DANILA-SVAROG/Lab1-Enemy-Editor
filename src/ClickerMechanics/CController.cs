using System.Windows;

namespace ClickerMechanics;

public class CController
{
    private readonly List<CCollectable> objects;
    private double spawnRate;
    private double time;
    private readonly Random rng;
    private readonly double minLifetime;
    private readonly double maxLifetime;
    private readonly double minSpriteSize;
    private readonly double maxSpriteSize;
    private Size sceneSize;
    private double points;
    private double timeLeft;
    private readonly bool timedRound;
    private bool running;
    private readonly CPlayer player;
    private readonly bool bonusesEnabled;
    private double lifetimeModifier = 1;

    public CController(double spawnRate, ulong startTime, Size sceneSize)
        : this(spawnRate, startTime, sceneSize, new CPlayer(0), false)
    {
    }

    public CController(double spawnRate, ulong startTime, Size sceneSize, bool bonusesEnabled)
        : this(spawnRate, startTime, sceneSize, new CPlayer(bonusesEnabled ? 0.45 : 0), bonusesEnabled)
    {
    }

    public CController(double spawnRate, ulong startTime, Size sceneSize, CPlayer player, bool bonusesEnabled)
    {
        if (!double.IsFinite(spawnRate) || spawnRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(spawnRate));
        }

        rng = new Random();
        objects = new List<CCollectable>();
        this.player = player;
        this.bonusesEnabled = bonusesEnabled;
        this.spawnRate = spawnRate;
        this.sceneSize = sceneSize;
        timeLeft = startTime;
        timedRound = startTime > 0;
        minLifetime = 1;
        maxLifetime = 5;
        minSpriteSize = 30;
        maxSpriteSize = 60;
    }

    public IReadOnlyList<CCollectable> getObjects() => objects.AsReadOnly();
    public CPlayer getPlayer() => player;
    public double getPoints() => points;
    public double getTimeLeft() => timeLeft;
    public double getSpawnRate() => spawnRate;
    public double getLifetimeModifier() => lifetimeModifier;
    public bool isRunning() => running;

    public void start()
    {
        running = true;
    }

    public void stop()
    {
        running = false;
        objects.Clear();
    }

    public void setSceneSize(Size value)
    {
        sceneSize = value;
        foreach (CCollectable obj in objects)
        {
            obj.fitToScene(sceneSize);
        }
    }

    public void spawnObject()
    {
        if (!running) return;
        double size = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);
        if (sceneSize.Width < size || sceneSize.Height < size)
        {
            return;
        }

        double lifetime = (minLifetime + rng.NextDouble() * (maxLifetime - minLifetime)) * lifetimeModifier;
        Point position = new Point(rng.NextDouble() * (sceneSize.Width - size), rng.NextDouble() * (sceneSize.Height - size));
        objects.Add(createObject(position, size, lifetime));
    }

    // Обычный контроллер создаёт фиолетовые сферы с очками.
    protected virtual CCollectable createObject(Point position, double size, double lifetime)
    {
        if (bonusesEnabled)
        {
            int type = rng.Next(10);
            if (type == 0) return new CClickSpeedUp(position, size, lifetime, 0.85);
            if (type == 1) return new CLifetimeChanger(position, size, lifetime, 1.2);
            if (type == 2) return new CSpawnRateChanger(position, size, lifetime, 0.85);
        }

        return new CPointGiver(position, size, lifetime);
    }

    public void destroyObject(CCollectable obj)
    {
        objects.Remove(obj);
    }

    // Обновляет круги до ближайшего появления или конца раунда.
    public void update(double delta)
    {
        if (!running || !double.IsFinite(delta) || delta <= 0)
        {
            return;
        }

        player.update(delta);

        while (delta > 0 && running)
        {
            double step = Math.Min(delta, Math.Max(0, spawnRate - time));
            if (timedRound) step = Math.Min(step, timeLeft);
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].UpdateLifetime(step))
                {
                    destroyObject(objects[i]);
                }
            }

            time += step;
            delta -= step;
            if (timedRound)
            {
                timeLeft = Math.Max(0, timeLeft - step);
                if (timeLeft <= 0.000001)
                {
                    timeLeft = 0;
                    stop();
                    break;
                }
            }

            if (time >= spawnRate - 0.000001)
            {
                time = 0;
                spawnObject();
            }
        }
    }

    // Сначала проверяет верхний круг, если круги перекрываются.
    public bool mouseClick(Point mousePosition)
    {
        if (!running || !player.canClick()) return false;
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i].onClick(player, this, mousePosition) == true)
            {
                player.tryClick();
                destroyObject(objects[i]);
                return true;
            }
        }

        return false;
    }

    public void pointsIncrease(double amount)
    {
        points += amount;
    }

    public void increaseLifetime(double modifier)
    {
        double newModifier = Math.Min(3, lifetimeModifier * modifier);
        double appliedModifier = newModifier / lifetimeModifier;
        lifetimeModifier = newModifier;
        foreach (CCollectable obj in objects)
        {
            obj.increaseLifetime(appliedModifier);
        }
    }

    public void increaseSpawnSpeed(double modifier)
    {
        spawnRate = Math.Max(0.15, spawnRate * modifier);
    }
}
