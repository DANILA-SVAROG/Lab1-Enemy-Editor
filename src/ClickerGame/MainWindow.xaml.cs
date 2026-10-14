using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Input;
using ClickerGame.Models;
using ClickerMechanics;
using EnemyEditor.Models;
using Microsoft.Win32;

namespace ClickerGame;

public partial class MainWindow : Window
{
    private readonly EnemyTemplateStore templates = new EnemyTemplateStore();
    private readonly Player player = new Player();
    private Enemy? currentEnemy;
    private int defeatedCount;
    private readonly DispatcherTimer timer = new DispatcherTimer();
    private readonly System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
    private CGameController? controller;

    public MainWindow()
    {
        InitializeComponent();
        UpdatePlayerInfo();
        timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        timer.Tick += Timer_Tick;
        Closed += (sender, e) => timer.Stop();
        clock.Start();
        timer.Start();
        if (File.Exists(EnemyFileLocation.FilePath))
        {
            LoadTemplates(EnemyFileLocation.FilePath);
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        UpdateGameTime();
    }

    private void UpdateGameTime()
    {
        double delta = clock.Elapsed.TotalSeconds;
        clock.Restart();
        if (controller is not null)
        {
            controller.update(delta);
            DrawBonuses();
        }
        else
        {
            player.update(delta);
        }
        if (currentEnemy is not null)
        {
            EnemyButton.IsEnabled = player.canClick() && !currentEnemy.IsDead;
        }

        UpdatePlayerInfo();
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
            controller?.stop();
            controller = new CGameController(player, new Size(EnemyScene.ActualWidth, EnemyScene.ActualHeight));
            controller.start();
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
        EnemyButton.IsEnabled = player.canClick() && !currentEnemy.IsDead;
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
        DamageTextBlock.Text = $"Урон: {player.DealDamage()}";
        CostTextBlock.Text = $"Цена улучшения: {player.UpgradeCost}";
        UpgradeButton.IsEnabled = player.Gold >= player.UpgradeCost;
        CooldownTextBlock.Text = $"Перезарядка: {player.getTimeBeforeClick():F2} с; осталось: {player.getRemainingCooldown():F2} с";
        CooldownCostTextBlock.Text = $"Цена ускорения: {player.CooldownUpgradeCost}";
        CooldownUpgradeButton.IsEnabled = player.CanUpgradeCooldown && player.Gold >= player.CooldownUpgradeCost;
        BonusesTextBlock.Text = player.GetBonuses().Count == 0 ? "Активных бонусов нет"
            : string.Join("\n", player.GetBonuses().Select(bonus => $"{bonus.getDescription()}: {bonus.getTime():F1} с"));
    }

    private void EnemyButton_Click(object sender, RoutedEventArgs e)
    {
        UpdateGameTime();
        if (currentEnemy is null || currentEnemy.IsDead || !player.tryClick())
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

        EnemyButton.IsEnabled = player.canClick();
    }

    private void UpgradeButton_Click(object sender, RoutedEventArgs e)
    {
        if (player.TryUpgrade())
        {
            StatusTextBlock.Text = $"Урон улучшен до {player.Damage}.";
            UpdatePlayerInfo();
        }
    }

    private void CooldownUpgradeButton_Click(object sender, RoutedEventArgs e)
    {
        if (player.TryUpgradeCooldown())
        {
            StatusTextBlock.Text = "Перезарядка атаки уменьшена.";
            UpdatePlayerInfo();
        }
    }

    private void EnemyScene_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        controller?.setSceneSize(e.NewSize);
    }

    private void BonusCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        UpdateGameTime();
        if (controller?.mouseClick(e.GetPosition(BonusCanvas)) == true)
        {
            StatusTextBlock.Text = "Бонус получен.";
            UpdatePlayerInfo();
            DrawBonuses();
            EnemyButton.IsEnabled = player.canClick();
        }

        e.Handled = true;
    }

    private void DrawBonuses()
    {
        BonusCanvas.Children.Clear();
        if (controller is null) return;
        foreach (CCollectable obj in controller.getObjects())
        {
            BonusCanvas.Children.Add(obj.getSprite());
        }
    }
}
