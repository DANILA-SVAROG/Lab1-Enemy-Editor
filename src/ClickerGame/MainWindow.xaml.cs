using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using EnemyEditor.Models;
using Microsoft.Win32;

namespace ClickerGame;

public partial class MainWindow : Window
{
    private readonly EnemyTemplateStore templates = new EnemyTemplateStore();
    private readonly Player player = new Player();
    private Enemy? currentEnemy;
    private int defeatedCount;

    public MainWindow()
    {
        InitializeComponent();
        UpdatePlayerInfo();
        if (File.Exists(EnemyFileLocation.FilePath))
        {
            LoadTemplates(EnemyFileLocation.FilePath);
        }
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            DefaultExt = ".json",
            Filter = "JSON files (*.json)|*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            LoadTemplates(dialog.FileName);
        }
    }

    // Загружает шаблоны и создаёт первого противника.
    private void LoadTemplates(string path)
    {
        try
        {
            templates.LoadFromJson(path);
            FilePathTextBlock.Text = path;
            SpawnNextEnemy();
            StatusTextBlock.Text = $"Загружено шаблонов: {templates.Count}. Нажмите на противника.";
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Не удалось загрузить противников: {exception.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    // Выбирает следующего противника с учётом шансов.
    private void SpawnNextEnemy()
    {
        CEnemyTemplate template = templates.ChooseNext();
        string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Icons", "Monsters", template.IconName);
        EnemyIcon icon = new EnemyIcon { Name = template.IconName, ImagePath = iconPath };
        currentEnemy = new Enemy(template, defeatedCount + 1, icon);
        EnemyImage.Source = File.Exists(iconPath) ? new BitmapImage(new Uri(iconPath)) : null;
        EnemyButton.IsEnabled = !currentEnemy.IsDead;
        UpdateEnemyInfo();
    }

    private void UpdateEnemyInfo()
    {
        if (currentEnemy is null)
        {
            return;
        }

        EnemyNameTextBlock.Text = $"Имя: {currentEnemy.Name}";
        EnemyHealthTextBlock.Text = $"Здоровье: {currentEnemy.CurrentHitPoints} / {currentEnemy.MaxHitPoints}";
        EnemyRewardTextBlock.Text = $"Награда: {currentEnemy.GoldReward}";
        DefeatedTextBlock.Text = $"Побеждено: {defeatedCount}";
    }

    private void UpdatePlayerInfo()
    {
        LevelTextBlock.Text = $"Уровень: {player.Lvl}";
        GoldTextBlock.Text = $"Золото: {player.Gold}";
        DamageTextBlock.Text = $"Урон: {player.Damage}";
        CostTextBlock.Text = $"Цена улучшения: {player.UpgradeCost}";
        UpgradeButton.IsEnabled = player.Gold >= player.UpgradeCost;
    }

    private void EnemyButton_Click(object sender, RoutedEventArgs e)
    {
        if (currentEnemy is null)
        {
            return;
        }

        if (currentEnemy.TakeDamage(player.DealDamage(), out BigNumber reward))
        {
            player.AddGold(reward);
            defeatedCount++;
            StatusTextBlock.Text = $"Победа! Получено золота: {reward}.";
            UpdatePlayerInfo();
            SpawnNextEnemy();
        }
        else
        {
            UpdateEnemyInfo();
        }
    }

    private void UpgradeButton_Click(object sender, RoutedEventArgs e)
    {
        if (player.TryUpgrade())
        {
            StatusTextBlock.Text = $"Урон улучшен до {player.Damage}.";
            UpdatePlayerInfo();
        }
    }
}
