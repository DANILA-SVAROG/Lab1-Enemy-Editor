namespace EnemyEditor.Models;

public class Enemy
{
    public string Name { get; private set; }
    public BigNumber MaxHitPoints { get; private set; }
    public BigNumber CurrentHitPoints { get; private set; }
    public BigNumber GoldReward { get; private set; }
    public bool IsDead { get; private set; }
    public EnemyIcon Icon { get; private set; }

    // Создаёт противника на сцене по шаблону.
    public Enemy(CEnemyTemplate template, int level, EnemyIcon icon)
    {
        Name = template.Name;
        Icon = icon;
        MaxHitPoints = new BigNumber(template.BaseLife.ToString());
        GoldReward = new BigNumber(template.BaseGold.ToString());
        for (int i = 1; i < level; i++)
        {
            MaxHitPoints = MaxHitPoints * template.LifeModifier;
            GoldReward = GoldReward * template.GoldModifier;
        }

        CurrentHitPoints = MaxHitPoints.Clone();
        IsDead = CurrentHitPoints == new BigNumber("0");
    }

    // Уменьшает здоровье и отдаёт награду при победе.
    public bool TakeDamage(BigNumber damage, out BigNumber goldReward)
    {
        goldReward = new BigNumber("0");
        if (IsDead)
        {
            return false;
        }

        if (damage >= CurrentHitPoints)
        {
            Die();
            goldReward = GoldReward;
            return true;
        }

        CurrentHitPoints = CurrentHitPoints - damage;
        return false;
    }

    private void Die()
    {
        CurrentHitPoints = new BigNumber("0");
        IsDead = true;
    }
}
