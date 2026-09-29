using System.IO;

namespace EnemyEditor.Models;

public class EnemyTemplateStore
{
    private readonly List<CEnemyTemplate> enemies = new List<CEnemyTemplate>();
    private readonly List<double> chances = new List<double>();
    private readonly Random rng = new Random();

    public int Count => enemies.Count;

    // Загружает шаблоны из файла редактора.
    public void LoadFromJson(string path)
    {
        CEnemyTemplateList list = new CEnemyTemplateList();
        list.LoadFromJson(path);
        if (list.Enemies.Count == 0)
        {
            throw new InvalidDataException("В файле нет противников.");
        }

        foreach (CEnemyTemplate enemy in list.Enemies)
        {
            if (string.IsNullOrWhiteSpace(enemy.Name) || string.IsNullOrWhiteSpace(enemy.IconName)
                || enemy.BaseLife <= 0 || enemy.BaseGold < 0
                || !double.IsFinite(enemy.LifeModifier) || enemy.LifeModifier <= 0
                || !double.IsFinite(enemy.GoldModifier) || enemy.GoldModifier <= 0
                || !double.IsFinite(enemy.SpawnChance) || enemy.SpawnChance < 0)
            {
                throw new InvalidDataException("В файле есть неверные данные противника.");
            }
        }

        enemies.Clear();
        enemies.AddRange(list.Enemies);
        NormalizeChances();
    }

    // Приводит сумму шансов к единице.
    private void NormalizeChances()
    {
        double sum = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            sum += enemies[i].SpawnChance;
        }

        if (!double.IsFinite(sum) || sum <= 0)
        {
            throw new InvalidDataException("Сумма шансов появления должна быть больше нуля.");
        }

        chances.Clear();
        for (int i = 0; i < enemies.Count; i++)
        {
            chances.Add(enemies[i].SpawnChance / sum);
        }
    }

    // Ищет шаблон по выпавшему случайному числу.
    private CEnemyTemplate FindByChance(double chance)
    {
        double sum = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            sum += chances[i];
            if (sum >= chance)
            {
                return enemies[i];
            }
        }

        return enemies[^1];
    }

    public CEnemyTemplate ChooseNext()
    {
        if (enemies.Count == 0)
        {
            throw new InvalidOperationException("Сначала загрузите шаблоны противников.");
        }

        return FindByChance(rng.NextDouble());
    }
}
