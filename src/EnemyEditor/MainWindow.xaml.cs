using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using EnemyEditor.Models;
using Microsoft.Win32;

namespace EnemyEditor;

public partial class MainWindow : Window
{
    private readonly CEnemyTemplateList enemyTemplates = new();
    private readonly List<EnemyIcon> enemyIcons = new();

    public MainWindow()
    {
        InitializeComponent();
        LoadDefaultIcons();
    }

    // Загружает иконки монстров из папки проекта.
    private void LoadDefaultIcons()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Assets", "Icons", "icons", "Monsters");
        if (Directory.Exists(path))
        {
            LoadIconsFromFolder(path);
            ShowIconsInList();
            IconsPathTextBlock.Text = "Иконки монстров";
        }
    }

    // Загружает все png-файлы из выбранной папки.
    public void LoadIconsFromFolder(string path)
    {
        string filter = "*.png";
        string[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
        enemyIcons.Clear();
        foreach (string file in files.OrderBy(file => file))
        {
            enemyIcons.Add(new EnemyIcon
            {
                Name = Path.GetFileName(file),
                ImagePath = file
            });
        }
    }

    // Заполняет ListBox изображениями.
    private void ShowIconsInList()
    {
        IconsListBox.Items.Clear();
        foreach (EnemyIcon icon in enemyIcons)
        {
            Image image = new()
            {
                Source = new BitmapImage(new Uri(icon.ImagePath)),
                Height = 64,
                Width = 64,
                Stretch = System.Windows.Media.Stretch.Uniform,
                Margin = new Thickness(5),
                ToolTip = icon.Name,
                Tag = icon
            };
            IconsListBox.Items.Add(image);
        }
    }

    // Открывает папку с иконками.
    private void LoadIconsButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog { Title = "Выберите папку с PNG-иконками" };
        if (dialog.ShowDialog() == true)
        {
            LoadIconsFromFolder(dialog.FolderName);
            ShowIconsInList();
            IconsPathTextBlock.Text = dialog.FolderName;
        }
    }

    // Запоминает выбранную иконку.
    private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListBox iconHolder = (ListBox)sender;
        if (iconHolder.SelectedItem is Image selectedImage && selectedImage.Tag is EnemyIcon selectedIcon)
        {
            IconNameTextBox.Text = selectedIcon.Name;
            MainEnemyIcon.Source = selectedImage.Source;
        }
    }

    // Выбирает противника из списка.
    private void EnemiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (EnemiesListBox.SelectedIndex >= 0)
        {
            CEnemyTemplate? enemy = enemyTemplates.GetEnemyByIndex(EnemiesListBox.SelectedIndex);
            if (enemy != null)
            {
                FillFields(enemy);
            }
        }
    }

    // Заполняет поля данными шаблона.
    private void FillFields(CEnemyTemplate enemy)
    {
        EnemyNameTextBox.Text = enemy.Name;
        IconNameTextBox.Text = enemy.IconName;
        BaseLifeTextBox.Text = enemy.BaseLife.ToString();
        LifeModifierTextBox.Text = enemy.LifeModifier.ToString();
        BaseGoldTextBox.Text = enemy.BaseGold.ToString();
        GoldModifierTextBox.Text = enemy.GoldModifier.ToString();
        SpawnChanceTextBox.Text = enemy.SpawnChance.ToString();
        SetMainIcon(enemy.IconName);
    }

    // Показывает иконку выбранного шаблона.
    private void SetMainIcon(string iconName)
    {
        EnemyIcon? icon = enemyIcons.FirstOrDefault(item => item.Name.Equals(iconName, StringComparison.OrdinalIgnoreCase));
        MainEnemyIcon.Source = icon == null ? null : new BitmapImage(new Uri(icon.ImagePath));
    }

    // Добавляет шаблон после проверки полей.
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryReadFields(out CEnemyTemplate? enemy))
        {
            return;
        }
        if (enemyTemplates.GetEnemyByName(enemy!.Name) != null)
        {
            ShowError("Противник с таким именем уже есть.");
            return;
        }
        enemyTemplates.AddEnemy(enemy.Name, enemy.IconName, enemy.BaseLife, enemy.LifeModifier, enemy.BaseGold, enemy.GoldModifier, enemy.SpawnChance);
        RefreshEnemyList(enemyTemplates.Enemies.Count - 1);
    }

    // Сохраняет изменения выбранного шаблона.
    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        if (EnemiesListBox.SelectedIndex < 0 || !TryReadFields(out CEnemyTemplate? enemy))
        {
            return;
        }
        CEnemyTemplate? sameName = enemyTemplates.GetEnemyByName(enemy!.Name);
        if (sameName != null && sameName != enemyTemplates.GetEnemyByIndex(EnemiesListBox.SelectedIndex))
        {
            ShowError("Противник с таким именем уже есть.");
            return;
        }
        int index = EnemiesListBox.SelectedIndex;
        enemyTemplates.UpdateEnemy(index, enemy);
        RefreshEnemyList(index);
    }

    // Удаляет выбранный шаблон.
    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        if (EnemiesListBox.SelectedIndex < 0)
        {
            ShowError("Сначала выберите противника.");
            return;
        }
        int index = EnemiesListBox.SelectedIndex;
        enemyTemplates.DeleteEnemyByIndex(index);
        RefreshEnemyList(Math.Min(index, enemyTemplates.Enemies.Count - 1));
    }

    // Обновляет список имён.
    private void RefreshEnemyList(int selectedIndex)
    {
        EnemiesListBox.ItemsSource = null;
        EnemiesListBox.ItemsSource = enemyTemplates.GetListOfEnemyNames();
        if (selectedIndex >= 0 && selectedIndex < EnemiesListBox.Items.Count)
        {
            EnemiesListBox.SelectedIndex = selectedIndex;
        }
        else
        {
            ClearFields();
        }
    }

    // Читает значения из формы.
    private bool TryReadFields(out CEnemyTemplate? enemy)
    {
        enemy = null;
        string name = EnemyNameTextBox.Text.Trim();
        string iconName = IconNameTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(iconName)
            || !int.TryParse(BaseLifeTextBox.Text, out int baseLife)
            || !double.TryParse(LifeModifierTextBox.Text, out double lifeModifier)
            || !int.TryParse(BaseGoldTextBox.Text, out int baseGold)
            || !double.TryParse(GoldModifierTextBox.Text, out double goldModifier)
            || !double.TryParse(SpawnChanceTextBox.Text, out double spawnChance))
        {
            ShowError("Заполните имя, иконку и числовые поля.");
            return false;
        }
        if (baseLife < 0 || baseGold < 0 || spawnChance < 0)
        {
            ShowError("Здоровье, золото и шанс не могут быть отрицательными.");
            return false;
        }
        enemy = new CEnemyTemplate(name, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance);
        return true;
    }

    // Очищает поля формы.
    private void ClearFields()
    {
        EnemyNameTextBox.Clear();
        IconNameTextBox.Clear();
        BaseLifeTextBox.Clear();
        LifeModifierTextBox.Clear();
        BaseGoldTextBox.Clear();
        GoldModifierTextBox.Clear();
        SpawnChanceTextBox.Clear();
        MainEnemyIcon.Source = null;
    }

    // Сохраняет список в выбранный файл.
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            FileName = "enemies.json",
            DefaultExt = ".json",
            Filter = "JSON files (*.json)|*.json"
        };
        if (dialog.ShowDialog() == true)
        {
            enemyTemplates.SaveToJson(dialog.FileName);
            MessageBox.Show("Список сохранён.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // Загружает список из выбранного файла.
    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = ".json",
            Filter = "JSON files (*.json)|*.json"
        };
        if (dialog.ShowDialog() == true)
        {
            try
            {
                enemyTemplates.LoadFromJson(dialog.FileName);
                RefreshEnemyList(0);
                MessageBox.Show("Список загружен.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                ShowError($"Не удалось загрузить JSON: {exception.Message}");
            }
        }
    }

    // Показывает сообщение об ошибке.
    private static void ShowError(string message)
    {
        MessageBox.Show(message, "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
