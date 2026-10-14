using ClickerMechanics;

namespace EnemyEditor.Models;

public class Player : CPlayer
{
    private readonly List<CBonusEffect> bonuses = new List<CBonusEffect>();
    public int Lvl { get; private set; }
    public BigNumber Gold { get; private set; }
    public BigNumber Damage { get; private set; }
    public double DamageModifier { get; private set; }
    public BigNumber UpgradeCost { get; private set; }
    public double UpgradeModifier { get; private set; }
    public BigNumber CooldownUpgradeCost { get; private set; }
    public bool CanUpgradeCooldown => base.getTimeBeforeClick() > 0.080001;

    public Player() : base(0.65)
    {
        Lvl = 1;
        Gold = new BigNumber("0");
        Damage = new BigNumber("5");
        DamageModifier = 1.5;
        UpgradeCost = new BigNumber("10");
        UpgradeModifier = 1.2;
        CooldownUpgradeCost = new BigNumber("15");
    }

    // Добавляет награду после победы.
    public void AddGold(BigNumber amount)
    {
        Gold = Gold + amount;
    }

    // Проверяет деньги и улучшает урон.
    public bool TryUpgrade()
    {
        if (!TrySpendGold(UpgradeCost))
        {
            return false;
        }

        Lvl++;
        RecalculateStats();
        return true;
    }

    public BigNumber DealDamage()
    {
        BigNumber result = Damage;
        foreach (CBonusEffect bonus in bonuses)
        {
            result = result * bonus.getDamageModifier();
        }

        return result;
    }

    public void AddBonus(CBonusEffect bonus)
    {
        bonuses.Add(bonus);
    }

    public IReadOnlyList<CBonusEffect> GetBonuses() => bonuses.AsReadOnly();

    public override double getTimeBeforeClick()
    {
        double result = base.getTimeBeforeClick();
        foreach (CBonusEffect bonus in bonuses)
        {
            result *= bonus.getCooldownModifier();
        }

        return Math.Max(0.08, result);
    }

    // У каждого временного эффекта свой срок окончания.
    public override void update(double delta)
    {
        base.update(delta);
        for (int i = bonuses.Count - 1; i >= 0; i--)
        {
            if (bonuses[i].update(delta))
            {
                bonuses.RemoveAt(i);
            }
        }
    }

    // За золото уменьшает обычную перезарядку атаки.
    public bool TryUpgradeCooldown()
    {
        if (!CanUpgradeCooldown || !TrySpendGold(CooldownUpgradeCost))
        {
            return false;
        }

        increaseSpeed(0.85);
        CooldownUpgradeCost = CooldownUpgradeCost * 1.5;
        return true;
    }

    private bool TrySpendGold(BigNumber amount)
    {
        if (Gold < amount)
        {
            return false;
        }

        Gold = Gold - amount;
        return true;
    }

    // Обновляет урон и стоимость следующего улучшения.
    private void RecalculateStats()
    {
        Damage = CalculateTotalDamage();
        UpgradeCost = CalculateNextUpgradeCost();
    }

    private BigNumber CalculateTotalDamage()
    {
        return Damage * DamageModifier;
    }

    private BigNumber CalculateNextUpgradeCost()
    {
        return UpgradeCost * (UpgradeModifier * (Lvl - 1));
    }
}
