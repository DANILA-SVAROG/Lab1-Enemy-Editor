using System.IO;

namespace EnemyEditor.Models;

public static class EnemyFileLocation
{
    // Общий файл позволяет редактору и игре работать с одними шаблонами.
    public static string FilePath
    {
        get
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EnemyEditor");
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "enemies.json");
        }
    }
}
