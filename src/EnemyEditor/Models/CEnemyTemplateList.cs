using System.IO;
using System.Text.Json;

namespace EnemyEditor.Models;

// Класс управляет списком шаблонов противников.
public class CEnemyTemplateList
{
    private List<CEnemyTemplate> enemies;

    public CEnemyTemplateList()
    {
        enemies = new List<CEnemyTemplate>();
    }

    public IReadOnlyList<CEnemyTemplate> Enemies => enemies;

    // Добавляет новый шаблон в список.
    public void AddEnemy(
        string name,
        string iconName,
        int baseLife,
        double lifeModifier,
        int baseGold,
        double goldModifier,
        double spawnChance)
    {
        enemies.Add(new CEnemyTemplate(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance));
    }

    // Возвращает противника по имени.
    public CEnemyTemplate? GetEnemyByName(string name)
    {
        return enemies.FirstOrDefault(enemy => enemy.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    // Возвращает противника по индексу.
    public CEnemyTemplate? GetEnemyByIndex(int id)
    {
        return id >= 0 && id < enemies.Count ? enemies[id] : null;
    }

    // Удаляет противника по имени.
    public void DeleteEnemyByName(string name)
    {
        var enemy = GetEnemyByName(name);
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }

    // Удаляет противника по индексу.
    public void DeleteEnemyByIndex(int id)
    {
        if (id >= 0 && id < enemies.Count)
        {
            enemies.RemoveAt(id);
        }
    }

    // Возвращает имена всех противников.
    public List<string> GetListOfEnemyNames()
    {
        return enemies.Select(enemy => enemy.Name).ToList();
    }

    // Заменяет шаблон по индексу.
    public void UpdateEnemy(int id, CEnemyTemplate enemy)
    {
        if (id >= 0 && id < enemies.Count)
        {
            enemies[id] = enemy;
        }
    }

    // Сохраняет список в JSON.
    public void SaveToJson(string path)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(path, JsonSerializer.Serialize(enemies, options));
    }

    // Загружает список из JSON.
    public void LoadFromJson(string path)
    {
        string jsonFromFile = File.ReadAllText(path);
        using JsonDocument doc = JsonDocument.Parse(jsonFromFile);
        var loadedEnemies = new List<CEnemyTemplate>();

        foreach (JsonElement element in doc.RootElement.EnumerateArray())
        {
            string name = element.GetProperty("Name").GetString() ?? string.Empty;
            string iconName = element.GetProperty("IconName").GetString() ?? string.Empty;
            int baseLife = element.GetProperty("BaseLife").GetInt32();
            double lifeModifier = element.GetProperty("LifeModifier").GetDouble();
            int baseGold = element.GetProperty("BaseGold").GetInt32();
            double goldModifier = element.GetProperty("GoldModifier").GetDouble();
            double spawnChance = element.GetProperty("SpawnChance").GetDouble();
            loadedEnemies.Add(new CEnemyTemplate(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance));
        }

        enemies = loadedEnemies;
    }
}
