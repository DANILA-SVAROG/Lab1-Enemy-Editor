namespace EnemyEditor.Models;

public class Player
{
    public int Lvl { get; private set; }
    public BigNumber Gold { get; private set; }
    public BigNumber Damage { get; private set; }
    public double DamageModifier { get; private set; }
    public BigNumber UpgradeCost { get; private set; }
    public double UpgradeModifier { get; private set; }

    public Player()
    {
        Lvl = 1;
        Gold = new BigNumber("0");
        Damage = new BigNumber("5");
        DamageModifier = 1.5;
        UpgradeCost = new BigNumber("10");
        UpgradeModifier = 1.2;
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
        return Damage;
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
